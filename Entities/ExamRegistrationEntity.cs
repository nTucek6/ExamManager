using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class ExamRegistrationEntity
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
    }
}
