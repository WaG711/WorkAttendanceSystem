using Microsoft.EntityFrameworkCore;
using WorkAttendanceSystem.Domain.Entities;
using WorkAttendanceSystem.Infrastructure.Configurations;

namespace WorkAttendanceSystem.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ShiftConfiguration());
        }
    }
}
