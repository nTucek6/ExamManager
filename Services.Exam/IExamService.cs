
namespace Services.Exam
{
    public interface IExamService
    {
       public Task<List<StudentExamsDTO>> GetStudentExams(int StudentId);

    }
}
