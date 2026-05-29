using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HairSalonVN.Database;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SalonDbContext>
{
    public SalonDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SalonDbContext>();

        var connectionString =
            "Server=localhost;Database=HairSalonDB;Trusted_Connection=True;TrustServerCertificate=True";

        optionsBuilder.UseSqlServer(connectionString);

        return new SalonDbContext(optionsBuilder.Options);
    }
}
