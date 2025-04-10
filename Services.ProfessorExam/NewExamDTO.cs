namespace Services.ProfessorExam
{
    public class NewExamDTO
    {
        public int? ExamId { get; set; }
        public int ProfessorId { get; set; }
        public int SubjectId { get; set; }
        public string ExamLocation { get; set; }
        public DateTime DeadlineDate { get; set; }

    }
}
