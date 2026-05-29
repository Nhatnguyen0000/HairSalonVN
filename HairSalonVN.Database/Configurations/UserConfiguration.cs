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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.FullName).HasMaxLength(100).IsRequired();
            b.Property(u => u.Email).HasMaxLength(200).IsRequired();
            b.HasIndex(u => u.Email).IsUnique();          // Email phải unique
            b.Property(u => u.PasswordHash).IsRequired();
            b.Property(u => u.Phone).HasMaxLength(15);
            b.Property(u => u.Role).HasMaxLength(20)
             .HasDefaultValue(Roles.Customer);
            b.Property(u => u.IsActive).HasDefaultValue(true);
        }
    }

}
