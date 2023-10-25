using Microsoft.EntityFrameworkCore;

namespace Complains-MVC.Model
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
       
        public DbSet<RegisterReq> registers { get; set; }
        public DbSet<FileComp> Files { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisterReq>()
                .HasNoKey();
        }

    }
}
