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
        
    }
}

