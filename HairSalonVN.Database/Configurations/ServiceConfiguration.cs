using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HairSalonVN.Database.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> b)
    {
        b.HasKey(s => s.Id);
        b.Property(s => s.Name).HasMaxLength(200).IsRequired();
        b.Property(s => s.Description).HasMaxLength(1000);
        b.Property(s => s.ImageUrl).HasMaxLength(500);
        b.Property(s => s.Price).HasPrecision(18, 2);
        b.Property(s => s.DurationMinutes).IsRequired();
        b.Property(s => s.IsActive).HasDefaultValue(true);
    }
}
