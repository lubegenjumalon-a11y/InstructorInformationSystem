
using InstructorInfoSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InstructorInfoSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
