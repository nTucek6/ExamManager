using DatabaseContext;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Exam
{
    public class ExamService : IExamService
    {
        private readonly ExamManagerContext database;

        public ExamService(ExamManagerContext database)
        {
            this.database = database;
        }

        public async Task<List<StudentExamsDTO>> GetStudentExams(int StudentId)
        {
            List<StudentExamsDTO> studentExams = await database.ExamRegistrations.Join(database.Exams, examReg => examReg.ExamId, exam => exam.Id, (examReg, exam) => new
            {
                examReg, exam
            }).Join(database.Subjects, combined => combined.exam.SubjectId, subject => subject.Id, (combined, subject) => new
            {
                ExamId = combined.exam.Id,
                combined.exam.ExamLocation,
                combined.exam.DeadlineDate,
                combined.exam.ApplicationsDate,
                combined.exam.CheckOutDate,
                SubjectId = subject.Id,
                SubjectName = subject.Subject,
                combined.examReg.StudentId,
            })    
            .Where(q => q.StudentId == StudentId && q.DeadlineDate.Date >= DateTime.Now.Date.ToUniversalTime())
            .Select(s => new StudentExamsDTO 
            {
                ExamId = s.ExamId,
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
                DeadlineDate = s.DeadlineDate,
                ApplicationsDate = s.ApplicationsDate,
                CheckOutDate = s.CheckOutDate,
                Location = s.ExamLocation,

            }).ToListAsync();

            return studentExams;
        }


      /*  public async Task<List<StudentExamsDTO>> GetStudentExams(int StudentId)
        {
            var exams = await database.ExamRegistrations.Where(w => w.StudentId == StudentId).ToListAsync();

            List<StudentExamsDTO> studentExams = new List<StudentExamsDTO>();

            foreach (var e in exams)
            {
                List<StudentExamsDTO> studentExams = await database.ExamRegistrations.Join(database.Exams, examReg => examReg.ExamId, exam => exam.Id, (examReg, exam) => new
                {
                    examReg,
                    exam
                }).Join(database.Subjects, combined => combined.exam.SubjectId, subject => subject.Id, (combined, subject) => new
                {
                    ExamId = combined.exam.Id,
                    combined.exam.ExamLocation,
                    combined.exam.DeadlineDate,
                    combined.exam.ApplicationsDate,
                    combined.exam.CheckOutDate,
                    SubjectId = subject.Id,
                    SubjectName = subject.Subject,
                    combined.examReg.StudentId,
                })
                .Where(q => q.StudentId == StudentId && q.DeadlineDate.Date >= DateTime.Now.Date.ToUniversalTime())
                .Select(s => new StudentExamsDTO
                {
                    ExamId = s.ExamId,
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName,
                    DeadlineDate = s.DeadlineDate,
                    ApplicationsDate = s.ApplicationsDate,
                    CheckOutDate = s.CheckOutDate,
                    Location = s.ExamLocation,

                    var exam = await database.Exams.Where(w => w.Id == e.ExamId && w.DeadlineDate.Date >= DateTime.Now.Date.ToUniversalTime()).FirstOrDefaultAsync();
                    if (exam != null)
                {
                    var subject = await database.Subjects.Where(w => w.Id == exam.SubjectId).FirstOrDefaultAsync();
                }).ToListAsync();

                studentExams.Add(new StudentExamsDTO
                {
                    ExamId = exam.Id,
                    SubjectId = subject.Id,
                    SubjectName = subject.Subject,
                    DeadlineDate = exam.DeadlineDate,
                    ApplicationsDate = exam.ApplicationsDate,
                    CheckOutDate = exam.CheckOutDate,
                    Location = exam.ExamLocation,
                });
            }
        }
             return studentExams;
         }

        */





        public async Task RegisterStudentExam(RegisterExamDTO studentRegisterDTO)
        {
            var checkForExam = await database.ExamRegistrations.Where(q => q.ExamId == studentRegisterDTO.ExamId && q.StudentId == studentRegisterDTO.StudentId).FirstOrDefaultAsync();
            if (checkForExam == null)
            {
                await database.ExamRegistrations.AddAsync(new ExamRegistrationEntity { ExamId = studentRegisterDTO.ExamId, StudentId = studentRegisterDTO.StudentId });
                await database.SaveChangesAsync();
            }
            else {
                throw new Exception("Exam is already registered!");
            } 
        }

        public async Task DeleteStudentExam(int ExamId, int StudentId)
        {
            var transaction = database.Database.BeginTransaction();

            try
            {
                var examRegisterRemove = await database.ExamRegistrations.Where(q => q.ExamId == ExamId && q.StudentId == StudentId).FirstOrDefaultAsync();
                if (examRegisterRemove != null)
                {
                    database.ExamRegistrations.Remove(examRegisterRemove);
                }
                await database.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }   
        }

        public async Task<List<StudentExamsDTO>> GetExamsForRegister(int StudentId)
        {
            // Return exams that student has registered for exclude
            var registeredExams = await database.ExamRegistrations.Where(q => q.StudentId == StudentId).Select(s => s.ExamId).ToListAsync();

          
            List<int> subjectsToExcludeId = await database.Exams.Where(q => registeredExams.Contains(q.Id) 
            && q.DeadlineDate.Date >= DateTime.Now.Date.ToUniversalTime())
                .Select(s => s.SubjectId).Distinct().ToListAsync();

            // Get subjects that student is registered for
            var studentSubjects = await database.StudentSubjects.Where(q => q.StudentId == StudentId && !subjectsToExcludeId.Contains(q.SubjectId)).Select(s => s.SubjectId).ToListAsync();

            var examsToRegister = await database.Exams.Where(q => studentSubjects.Contains(q.SubjectId) && q.ApplicationsDate >= DateTime.Now.Date.ToUniversalTime()).ToListAsync();

            List<StudentExamsDTO> studentExams = new List<StudentExamsDTO>();

            if(examsToRegister.Count > 0)
            {
                foreach (var exam in examsToRegister)
                {
                    var subject = await database.Subjects.Where(w => w.Id == exam.SubjectId).FirstOrDefaultAsync();

                    studentExams.Add(new StudentExamsDTO
                    {
                        ExamId = exam.Id,
                        SubjectId = subject.Id,
                        SubjectName = subject.Subject,
                        DeadlineDate = exam.DeadlineDate,
                        ApplicationsDate = exam.ApplicationsDate,
                        CheckOutDate = exam.CheckOutDate,
                        Location = exam.ExamLocation,
                    });
                }
            }
            return studentExams;
        }

        public async Task<List<StudentExamsDTO>> GetAllStudentExams(int StudentId, string? Search)
        {
            Expression<Func<SubjectsEntity, bool>> predicate = x => true;

            if (!String.IsNullOrEmpty(Search))
            {
                predicate = x => x.Subject.ToLower().Contains(Search.ToLower().Trim());
            }

            List<StudentExamsDTO> AllExams = await database.StudentSubjects
                .Join(database.Subjects.Where(predicate), studentSubject => studentSubject.SubjectId, subject => subject.Id, (studentSubject, subject) => new { studentSubject, subject })
                .Join(database.Exams, combine => combine.subject.Id, exam => exam.SubjectId, (combine, exam) => new
                {
                    combine.studentSubject.StudentId,
                    combine.studentSubject.SubjectId,
                    SubjectName = combine.subject.Subject,
                    ExamId = exam.Id,
                    exam.ApplicationsDate,
                    exam.DeadlineDate,
                    exam.CheckOutDate,
                    Location = exam.ExamLocation,
                    combine.subject.Semester,
                })
                .Where(q => q.StudentId == StudentId)
                .Select(s =>
                new StudentExamsDTO
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName,
                    ExamId = s.ExamId,
                    ApplicationsDate = s.ApplicationsDate,
                    DeadlineDate = s.DeadlineDate,
                    CheckOutDate = s.CheckOutDate,
                    Location = s.Location,
                    Semester = s.Semester,
                })
                .OrderBy(o => o.Semester)
                .ThenBy(o => o.SubjectName)
                .ThenBy(o => o.DeadlineDate)
                .ToListAsync();

            return AllExams;

        }
     

     /*   public async Task<List<StudentExamsDTO>> GetAllStudentExams(int StudentId, string? Search)
        {
            Expression<Func<SubjectsEntity, bool>> predicate = x => true;

            if (!String.IsNullOrEmpty(Search))
            {
                predicate = x => x.Subject.ToLower().Contains(Search.ToLower().Trim());
            }


            var studentSubjects = await database.StudentSubjects.Where(q => q.StudentId == StudentId).Select(s => s.SubjectId).ToListAsync();

            var subjects = await database.Subjects.Where(q => studentSubjects.Contains(q.Id)).Where(predicate).ToListAsync();

            List <StudentExamsDTO> AllExams = new List<StudentExamsDTO>();

            foreach (var studentSubject in subjects) {
            var exams = await database.Exams.Where(q => q.SubjectId == studentSubject.Id).OrderBy(o => o.DeadlineDate).ToListAsync();
                if (exams.Count > 0)
                {
                    foreach (var exam in exams) {
                        AllExams.Add(new StudentExamsDTO
                        {
                            SubjectId = studentSubject.Id,
                            SubjectName = studentSubject.Subject,
                            ExamId = exam.Id,
                            ApplicationsDate = exam.ApplicationsDate,
                            DeadlineDate = exam.DeadlineDate,
                            CheckOutDate = exam.CheckOutDate,
                            Location = exam.ExamLocation,
                        });
                     }
                }
            }
            return AllExams;           
        } */
    }
}
