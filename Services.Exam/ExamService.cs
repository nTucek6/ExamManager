using DatabaseContext;
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
