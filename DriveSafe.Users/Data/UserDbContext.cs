using DriveSafe.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace DriveSafe.Users.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            // Agregar configuraciones adicionales si es necesario
        }
    }
}