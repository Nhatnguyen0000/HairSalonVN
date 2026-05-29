using HairSalonVN.Database.Constants;
using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonVN.Database.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> b)
        {
            b.HasKey(a => a.Id);
            b.Property(a => a.Status).HasMaxLength(20)
             .HasDefaultValue(AppointmentStatus.Pending);
            b.Property(a => a.BookingCode).HasMaxLength(20).IsRequired();
            b.Property(a => a.Notes).HasMaxLength(500);

            // ── Foreign Keys ───────────────────────────────────────
            b.HasOne(a => a.Customer)
             .WithMany(u => u.Appointments)
             .HasForeignKey(a => a.CustomerId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(a => a.Staff)
             .WithMany(s => s.Appointments)
             .HasForeignKey(a => a.StaffId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(a => a.Service)
             .WithMany(s => s.Appointments)
             .HasForeignKey(a => a.ServiceId)
             .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes để tăng tốc query kiểm tra slot ────────────
            b.HasIndex(a => new { a.StaffId, a.AppointmentDate });
            b.HasIndex(a => a.BookingCode).IsUnique();
            b.HasIndex(a => a.CustomerId);
            b.HasIndex(a => a.Status);
        }
    }

}
