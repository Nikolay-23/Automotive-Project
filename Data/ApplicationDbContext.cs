using Automotive_Project.Extensions;
using Automotive_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Automotive_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                        .HasKey(u => u.Id);

            modelBuilder.Entity<UserAccount>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("UserRoles"));

            modelBuilder.Seed();
        }
    }
}
