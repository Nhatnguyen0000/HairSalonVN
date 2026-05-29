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
    public class StaffServiceConfiguration : IEntityTypeConfiguration<StaffService>
    {
        public void Configure(EntityTypeBuilder<StaffService> b)
        {
            // Composite Primary Key
            b.HasKey(ss => new { ss.StaffId, ss.ServiceId });

            b.HasOne(ss => ss.Staff)
             .WithMany(s => s.StaffServices)
             .HasForeignKey(ss => ss.StaffId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(ss => ss.Service)
             .WithMany(s => s.StaffServices)
             .HasForeignKey(ss => ss.ServiceId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
