using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasAnnotation("Index", true); // Add unique index for email

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Create unique index on Email
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // Configure EmployeeRole entity
            modelBuilder.Entity<EmployeeRole>(entity =>
            {
                entity.HasKey(er => er.Id);

                entity.HasOne(er => er.Employee)
                    .WithMany(e => e.EmployeeRoles)
                    .HasForeignKey(er => er.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(er => er.Role)
                    .WithMany(r => r.EmployeeRoles)
                    .HasForeignKey(er => er.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed initial roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "HR" },
                new Role { Id = 3, RoleName = "Developer" }
            );
        }
    }
}
