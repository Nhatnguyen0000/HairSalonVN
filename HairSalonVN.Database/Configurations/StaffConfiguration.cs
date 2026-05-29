using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HairSalonVN.Database.Configurations;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> b)
    {
        b.HasKey(s => s.Id);
        b.Property(s => s.Specialty).HasMaxLength(200);
        b.Property(s => s.Bio).HasMaxLength(1000);
        b.Property(s => s.AvatarUrl).HasMaxLength(500);
        b.Property(s => s.IsAvailable).HasDefaultValue(true);
        b.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        b.HasOne(s => s.User)
         .WithOne(u => u.Staff)
         .HasForeignKey<Staff>(s => s.UserId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(s => s.UserId).IsUnique();
    }
}
