using DatabaseContext;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Services.ProfessorExam
{
    public class ProfessorExamService : IProfessorExamService
    {
        private readonly ExamManagerContext database;

        private readonly int CHECKOUT_DATE_SUBTRACTER = -1;
        private readonly int APPLICATION_DATE_SUBTRACTER = -5;
        
        public ProfessorExamService(ExamManagerContext database)
        {
            this.database = database;
        }

        public async Task<List<ProfessorExamsDTO>> GetProfessorExams(int ProfessorId)
        {
            var subjects = await database.Subjects.Where(q => q.ProfessorId == ProfessorId).ToListAsync();

            List<ProfessorExamsDTO> exams = new List<ProfessorExamsDTO>();

            foreach (var subject in subjects) {
              List<ExamsEntity> examsDb = await database.Exams.Where(q => q.SubjectId == subject.Id).OrderBy(o => o.DeadlineDate).ToListAsync();

                if (examsDb.Count > 0) {
                    foreach (var exam in examsDb) {
                        exams.Add(new ProfessorExamsDTO
                        {
                            ExamId = exam.Id,
                            SubjectId = subject.Id,
                            SubjectName = subject.Subject,
                            ExamLocation = exam.ExamLocation,
                            ApplicationsDate = exam.ApplicationsDate,
                            DeadlineDate = exam.DeadlineDate,
                            CheckOutDate = exam.CheckOutDate,
                        });
                    }
                }
            }
            return exams;
        }

        public async Task CreateExamPeriod(NewExamDTO newExamDTO)
        {
            try
            {
                DateTime ApplicationDate = new DateTime(newExamDTO.DeadlineDate.Year, newExamDTO.DeadlineDate.Month, newExamDTO.DeadlineDate.Day, 23, 59, 00).AddDays(APPLICATION_DATE_SUBTRACTER).ToUniversalTime();
                DateTime CheckOutDate = new DateTime(newExamDTO.DeadlineDate.Year, newExamDTO.DeadlineDate.Month, newExamDTO.DeadlineDate.Day, 23, 59, 00).AddDays(CHECKOUT_DATE_SUBTRACTER).ToUniversalTime();
                DateTime DeadlineDate = new DateTime(newExamDTO.DeadlineDate.Year, newExamDTO.DeadlineDate.Month, newExamDTO.DeadlineDate.Day, newExamDTO.DeadlineDate.Hour, newExamDTO.DeadlineDate.Minute, newExamDTO.DeadlineDate.Second).ToUniversalTime();

                await database.Exams.AddAsync(
                    new ExamsEntity
                    {
                        SubjectId = newExamDTO.SubjectId,
                        ExamLocation = newExamDTO.ExamLocation,
                        DeadlineDate = DeadlineDate,
                        ApplicationsDate = ApplicationDate,
                        CheckOutDate = CheckOutDate,
                    });
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
            }
        }
        public async Task UpdateExamPeriod(NewExamDTO exam)
        {
            var examDb = await database.Exams.Where(q=> q.Id == exam.ExamId).FirstOrDefaultAsync();
            if (examDb != null) {

                //if deadline is changed, this date need to be changed
                if (examDb.DeadlineDate != exam.DeadlineDate)
                {
                    examDb.DeadlineDate = new DateTime(exam.DeadlineDate.Year, exam.DeadlineDate.Month, exam.DeadlineDate.Day, exam.DeadlineDate.Hour, exam.DeadlineDate.Minute, exam.DeadlineDate.Second).ToUniversalTime();

                    DateTime ApplicationDate = new DateTime(exam.DeadlineDate.Year, exam.DeadlineDate.Month, exam.DeadlineDate.Day, 23, 59, 00).AddDays(APPLICATION_DATE_SUBTRACTER).ToUniversalTime();
                    DateTime CheckOutDate = new DateTime(exam.DeadlineDate.Year, exam.DeadlineDate.Month, exam.DeadlineDate.Day, 23, 59, 00).AddDays(CHECKOUT_DATE_SUBTRACTER).ToUniversalTime();
                    examDb.CheckOutDate = CheckOutDate;
                    examDb.ApplicationsDate = ApplicationDate;
                }
               
                //if subject is changed, remove all registered exams by students
                if (examDb.SubjectId != exam.SubjectId)
                {
                    List<ExamRegistrationEntity> examRegistration = await database.ExamRegistrations.Where(q=> q.ExamId == examDb.Id).ToListAsync();
                    database.ExamRegistrations.RemoveRange(examRegistration);

                    exam.SubjectId = exam.SubjectId;
                }

                if (examDb.ExamLocation != exam.ExamLocation)
                {
                    examDb.ExamLocation = exam.ExamLocation;
                }
 
                await database.SaveChangesAsync();
            }
        }

        public async Task DeleteExamPeriod(int ExamId)
        {
            var transaction = database.Database.BeginTransaction();

            try
            {
                var examRemove = await database.Exams.Where(q => q.Id == ExamId).FirstOrDefaultAsync();
                if (examRemove != null)
                {
                    database.Exams.Remove(examRemove);
                    List<ExamRegistrationEntity> examRegistrations = await database.ExamRegistrations.Where(q => q.ExamId == ExamId).ToListAsync();

                    if (examRegistrations.Count > 0)
                    {
                        database.ExamRegistrations.RemoveRange(examRegistrations);
                    }
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

        public async Task<ProfessorExamsDTO> GetExamPeriod(int ExamId)
        {
            var exam = await database.Exams.Where(q => q.Id == ExamId).FirstOrDefaultAsync();

            if (exam != null)
            {
                SubjectsEntity subject = await database.Subjects.Where(q => q.Id == exam.SubjectId).FirstOrDefaultAsync();
                if(subject != null)
                return new ProfessorExamsDTO
                {
                    ExamId = exam.Id,
                    SubjectId = subject.Id,
                    SubjectName = subject.Subject,
                    ExamLocation = exam.ExamLocation,
                    ApplicationsDate = exam.ApplicationsDate,
                    DeadlineDate = exam.DeadlineDate,
                    CheckOutDate = exam.CheckOutDate,
                };
            }
            return null;
        }

        public async Task<List<SelectSubjectDTO>> GetProfessorSubjects(int ProfessorId)
        {
            var subjects = await database.Subjects.Where(q=> q.ProfessorId == ProfessorId).Select(s=> new SelectSubjectDTO { value = s.Id, label=s.Subject}).ToListAsync();
            return subjects;
        }

        public async Task<List<StudentExamSelectDTO>> GetProfessorExamStudents(int ExamId)
        {
            var exam = await database.Exams.Where(q => q.Id == ExamId).FirstOrDefaultAsync();

            List<StudentExamSelectDTO> students = new List<StudentExamSelectDTO>();

            if(exam != null)
            {
                var registeredExams = await database.ExamRegistrations.Where(q => q.ExamId == exam.Id).ToListAsync();

                if(registeredExams.Count > 0)
                {
                    foreach(var registeredExam in registeredExams)
                    {
                        var student = await database.Users.Where(q => q.Id == registeredExam.StudentId).FirstOrDefaultAsync();
                        if(student != null)
                        {
                            students.Add(new StudentExamSelectDTO { Email = student.Email, Name = student.Name, Surname = student.Surname});
                        }
                    }
                }
            }
            return students;
        }
    }
}
