using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkAttendanceSystem.Domain.Entities;

namespace WorkAttendanceSystem.Infrastructure.Configurations
{
    public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {
            builder.ToTable("Shifts");

            builder.HasKey(s => s.Id);

            builder.OwnsOne(s => s.Period, period =>
            {
                period.Property(p => p.Start)
                      .HasColumnName("StartTime")
                      .IsRequired();

                period.Property(p => p.End)
                      .HasColumnName("EndTime");
            });

            builder.OwnsOne(s => s.Hours, hours =>
            {
                hours.Property(h => h.Value)
                     .HasColumnName("WorkedHours");
            });
        }
    }
}
