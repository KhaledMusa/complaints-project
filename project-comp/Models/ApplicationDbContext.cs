using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace project_comp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<RegisterReq> registers { get; set; }
        public DbSet<FileComp> Files { get; set; }
        public DbSet<Demand> Demands { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisterReq>()
                .HasNoKey();

            modelBuilder.Entity<User>().HasData(
         new User
         {
             Id = 1,
             UserName = "Admin",
             Phone = "077777777",

             Email = "admin@admin.com",
             Password = "123", 
             PasswordConfirmation = "123",
             TypeOfUser = "Admin"
         },
         new User
         {
             Id = 2,
             UserName = "User",
             Phone = "077778888",

             Email = "user@user.com",
             Password = "123", 
             PasswordConfirmation = "123",
             TypeOfUser = "User"
         }
            );
        }
    }
}