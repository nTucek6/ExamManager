using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SubjectsEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Subject { get; set; }
        public int ECTS { get; set; }
        public required int ProfessorId { get; set; }
        public required string Semester  { get; set; }
    }
}
