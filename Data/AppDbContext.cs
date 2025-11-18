using cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace cloud.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PhoneVerification> PhoneVerifications { get; set; }
        public DbSet<Models.File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PhoneVerification>()
                .ToTable("phone_verifications")
            ;

            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasIndex(u => u.phone)
                .IsUnique()
            ;
        }
    }
}
