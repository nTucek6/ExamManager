namespace Services.ProfessorExam
{
    public interface IProfessorExamService
    {
        public Task<List<ProfessorExamsDTO>> GetProfessorExams(int ProfessorId);
        public Task CreateExamPeriod(NewExamDTO newExamDTO);
        public Task UpdateExamPeriod(ProfessorExamsDTO exam);
        public Task DeleteExamPeriod(int ExamId);

    }
}
