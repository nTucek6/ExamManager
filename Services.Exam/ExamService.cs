using DatabaseContext;
using Entities;
using Microsoft.EntityFrameworkCore;

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
            var exams = await database.ExamRegistrations.Where(w => w.StudentId == StudentId).ToListAsync();

            List<StudentExamsDTO> studentExams = new List<StudentExamsDTO>();

            foreach (var e in exams) { 

                var exam = await database.Exams.Where(w=> w.Id == e.ExamId).FirstOrDefaultAsync();
                if(exam != null)
                {
                    var subject = await database.Subjects.Where(w => w.Id == exam.SubjectId).FirstOrDefaultAsync();

                    studentExams.Add(new StudentExamsDTO {
                        ExamId = exam.Id,
                        SubjectId = subject.Id,
                        SubjectName = subject.Subject,
                        DeadlineDate = exam.DeadlineDate,
                        ApplicationsDate = exam.ApplicationsDate,
                        CheckOutDate = exam.CheckOutDate,
                    });
                }
            }
            return studentExams;
        }

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

            List<int> subjectsToExcludeId = await database.Exams.Where(q => registeredExams.Contains(q.Id)).Select(s => s.SubjectId).Distinct().ToListAsync();

            // Get subjects that student is registered for
            var studentSubjects = await database.StudentSubjects.Where(q => q.StudentId == StudentId && !subjectsToExcludeId.Contains(q.SubjectId)).Select(s => s.SubjectId).ToListAsync();

            var examsToRegister = await database.Exams.Where(q => studentSubjects.Contains(q.SubjectId)).ToListAsync();

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
                    });
                }
            }
            return studentExams;
           
        }
    }
}
