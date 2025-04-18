namespace Services.Exam
{
    public interface IExamService
    {
       public Task<List<StudentExamsDTO>> GetStudentExams(int StudentId);
       public Task<List<StudentExamsDTO>> GetAllStudentExams(int StudentId, string? Search);
       public Task RegisterStudentExam (RegisterExamDTO studentRegisterDTO);
       public Task DeleteStudentExam(int ExamId, int StudentId);
       public Task<List<StudentExamsDTO>> GetExamsForRegister(int StudentId);

    }
}
