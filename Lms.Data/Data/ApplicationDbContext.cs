using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Lms.Core.Models.Entities.Course> Course { get; set; }
        public DbSet<Lms.Core.Models.Entities.Module> Module { get; set; }
    }
}
