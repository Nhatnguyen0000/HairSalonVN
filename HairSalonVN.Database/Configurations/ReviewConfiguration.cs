using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HairSalonVN.Database.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> b)
    {
        b.HasKey(r => r.Id);
        b.Property(r => r.Rating).IsRequired();
        b.Property(r => r.Comment).HasMaxLength(1000);

        b.HasIndex(r => r.AppointmentId).IsUnique();

        b.HasOne(r => r.Appointment)
         .WithOne(a => a.Review)
         .HasForeignKey<Review>(r => r.AppointmentId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(r => r.Customer)
         .WithMany()
         .HasForeignKey(r => r.CustomerId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
