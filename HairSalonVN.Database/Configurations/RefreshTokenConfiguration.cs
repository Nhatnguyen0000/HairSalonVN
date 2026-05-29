using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HairSalonVN.Database.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> b)
    {
        b.HasKey(r => r.Id);
        b.Property(r => r.Token).IsRequired().HasMaxLength(500);
        b.Property(r => r.ExpiresAt).IsRequired();
        b.Property(r => r.IsRevoked).HasDefaultValue(false);
        b.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        b.HasOne(r => r.User)
         .WithMany(u => u.RefreshTokens)
         .HasForeignKey(r => r.UserId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(r => r.Token).IsUnique();
    }
}
