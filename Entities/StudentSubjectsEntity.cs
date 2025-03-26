using System.ComponentModel.DataAnnotations;
namespace Entities
{
   public class StudentSubjectsEntity
    {
        [Key]
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
    }
}
