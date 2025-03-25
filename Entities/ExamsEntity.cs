using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class ExamsEntity
    {
        [Key]
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public DateTime DeadlineDate { get; set; }
        public DateTime ApplicationsDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
