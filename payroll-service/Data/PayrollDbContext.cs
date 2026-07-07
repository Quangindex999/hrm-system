using Microsoft.EntityFrameworkCore;
using payroll_service.Models;

namespace payroll_service.Data
{
    public class PayrollDbContext : DbContext
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options)
            : base(options)
        {
        }

        public DbSet<Payroll> Payrolls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình charset UTF-8 cho MySQL
            modelBuilder.Entity<Payroll>()
                .Property(p => p.EmployeeName)
                .HasCharSet("utf8mb4");

            // Thêm index
            modelBuilder.Entity<Payroll>()
                .HasIndex(p => p.EmployeeId)
                .IsUnique(false);

            modelBuilder.Entity<Payroll>()
                .HasIndex(p => p.CreatedAt);
        }
    }
}
