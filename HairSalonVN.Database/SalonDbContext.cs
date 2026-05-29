
using HairSalonVN.Database.Seeders;
using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalonVN.Database;

public class SalonDbContext : DbContext
{
    public SalonDbContext(DbContextOptions<SalonDbContext> options)
        : base(options) { }

    // ── DbSets ────────────────────────────────────────────────
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<StaffService> StaffServices => Set<StaffService>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<WorkingHour> WorkingHours => Set<WorkingHour>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Áp dụng TẤT CẢ IEntityTypeConfiguration trong assembly
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SalonDbContext).Assembly);

        // Seed dữ liệu ban đầu
        DataSeeder.Seed(modelBuilder);
    }

    // Override để thêm audit trail nếu cần sau này
    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);
}
