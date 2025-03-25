﻿using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SubjectsEntity
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public int ECTS { get; set; }
        public int ProfessorId { get; set; }
        public int Semester  { get; set; }

    }
}
