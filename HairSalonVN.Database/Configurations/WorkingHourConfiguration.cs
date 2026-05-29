using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HairSalonVN.Database.Configurations;

public class WorkingHourConfiguration : IEntityTypeConfiguration<WorkingHour>
{
    public void Configure(EntityTypeBuilder<WorkingHour> b)
    {
        b.HasKey(w => w.Id);
        b.Property(w => w.DayOfWeek).IsRequired();
        b.Property(w => w.StartTime).IsRequired();
        b.Property(w => w.EndTime).IsRequired();

        b.HasOne(w => w.Staff)
         .WithMany(s => s.WorkingHours)
         .HasForeignKey(w => w.StaffId)
         .OnDelete(DeleteBehavior.Cascade);

        // Mỗi Staff chỉ có 1 WorkingHour cho mỗi ngày trong tuần
        b.HasIndex(w => new { w.StaffId, w.DayOfWeek }).IsUnique();
    }
}
