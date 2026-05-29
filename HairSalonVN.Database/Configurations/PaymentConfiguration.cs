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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> b)
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            b.Property(p => p.Method).HasMaxLength(20);
            b.Property(p => p.Status).HasMaxLength(20);

            b.HasOne(p => p.Appointment)
             .WithOne(a => a.Payment)
             .HasForeignKey<Payment>(p => p.AppointmentId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
