namespace Services.ProfessorExam
{
   public class ProfessorExamsDTO
    {
        public int ExamId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string ExamLocation { get; set; }
        public DateTime DeadlineDate { get; set; }
        public DateTime ApplicationsDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
