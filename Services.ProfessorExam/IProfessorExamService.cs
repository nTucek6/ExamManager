namespace Services.ProfessorExam
{
    public interface IProfessorExamService
    {
        public Task<List<ProfessorExamsDTO>> GetProfessorExams(int ProfessorId);
        public Task CreateExamPeriod(NewExamDTO newExamDTO);
        public Task UpdateExamPeriod(NewExamDTO exam);
        public Task DeleteExamPeriod(int ExamId);
        public Task<ProfessorExamsDTO> GetExamPeriod(int ExamId);
        public Task<List<SelectSubjectDTO>> GetProfessorSubjects(int ProfessorId);
        public Task<List<StudentExamDTO>> GetProfessorExamStudents(int ExamId);

    }
}
