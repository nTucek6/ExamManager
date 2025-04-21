namespace Services.Exam
{
    public class StudentExamsDTO 
    {
        public int ExamId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Location { get; set; }
        public DateTime DeadlineDate { get; set; }
        public DateTime ApplicationsDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int? Semester {  get; set; }
    }
}
