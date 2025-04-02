using DatabaseContext;
using Entities;
using Microsoft.EntityFrameworkCore;

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
              ExamsEntity exam = await database.Exams.Where(q => q.SubjectId == subject.Id).FirstOrDefaultAsync();
                if (exam != null) { 
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
            return exams;
        }

        public async Task CreateExamPeriod(NewExamDTO newExamDTO)
        {
            DateTime ApplicationDate = new DateTime(newExamDTO.DeadlineDate.Year,newExamDTO.DeadlineDate.Month,newExamDTO.DeadlineDate.Day, 23, 59, 00).AddDays(APPLICATION_DATE_SUBTRACTER);
            DateTime CheckOutDate = new DateTime(newExamDTO.DeadlineDate.Year, newExamDTO.DeadlineDate.Month, newExamDTO.DeadlineDate.Day, 23, 59, 00).AddDays(CHECKOUT_DATE_SUBTRACTER);

            await database.Exams.AddAsync(
                new ExamsEntity
            {
                SubjectId = newExamDTO.SubjectId,
                ExamLocation = newExamDTO.ExamLocation,
                DeadlineDate = newExamDTO.DeadlineDate,
                ApplicationsDate = ApplicationDate,
                CheckOutDate = CheckOutDate,
            });
            await database.SaveChangesAsync();
        }
        public async Task UpdateExamPeriod(ProfessorExamsDTO exam)
        {
            var examDb = await database.Exams.Where(q=> q.Id == exam.ExamId).FirstOrDefaultAsync();
            if (examDb != null) {

                //if deadline is changed, this date need to be changed
                if (examDb.DeadlineDate != exam.DeadlineDate)
                {
                    DateTime ApplicationDate = new DateTime(exam.DeadlineDate.Year, exam.DeadlineDate.Month, exam.DeadlineDate.Day, 23, 59, 00).AddDays(APPLICATION_DATE_SUBTRACTER);
                    DateTime CheckOutDate = new DateTime(exam.DeadlineDate.Year, exam.DeadlineDate.Month, exam.DeadlineDate.Day, 23, 59, 00).AddDays(CHECKOUT_DATE_SUBTRACTER);
                    examDb.CheckOutDate = exam.CheckOutDate;
                    examDb.ApplicationsDate = exam.ApplicationsDate;
                }
               
                //if subject is changed, remove all registered exams by students
                if (examDb.SubjectId != exam.SubjectId)
                {
                    List<ExamRegistrationEntity> examRegistration = await database.ExamRegistrations.Where(q=> q.ExamId == examDb.Id).ToListAsync();
                    database.ExamRegistrations.RemoveRange(examRegistration);

                    exam.SubjectId = exam.SubjectId;
                }

                examDb.ExamLocation = exam.ExamLocation;
                examDb.DeadlineDate = exam.DeadlineDate;

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

    }
}
