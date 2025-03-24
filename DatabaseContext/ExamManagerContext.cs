using Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext
{
    public class ExamManagerContext : DbContext
    {
        public ExamManagerContext(DbContextOptions<ExamManagerContext> options) : base(options)
        {
        }
        
        public DbSet<UsersEntity> Users { get; set; }
        public DbSet<SubjectsEntity> Subjects { get; set; }
        public DbSet<ExamsEntity> Exams { get; set; }
        public DbSet<ExamRegistrationEntity> ExamRegistrations { get; set; }
    }
}

