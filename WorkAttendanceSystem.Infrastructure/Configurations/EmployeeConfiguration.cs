using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkAttendanceSystem.Domain.Entities;

namespace WorkAttendanceSystem.Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.OwnsOne(e => e.Name, name =>
            {
                name.Property(n => n.LastName)
                    .HasColumnName("LastName")
                    .IsRequired();

                name.Property(n => n.FirstName)
                    .HasColumnName("FirstName")
                    .IsRequired();

                name.Property(n => n.MiddleName)
                    .HasColumnName("MiddleName")
                    .IsRequired();
            });

            builder.OwnsOne(e => e.Position, pos =>
            {
                pos.Property(p => p.Name)
                   .HasColumnName("Position")
                   .IsRequired();
            });

            builder.HasMany(e => e.Shifts)
                   .WithOne(s => s.Employee)
                   .HasForeignKey(s => s.EmployeeId);
        }
    }
}
