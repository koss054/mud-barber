using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MudBarber.ApiService.Data;

public class MudBarberDbContextFactory : IDesignTimeDbContextFactory<MudBarberDbContext>
{
    public MudBarberDbContext CreateDbContext(string[] args)
    {
        // Connection string is not used to connect.
        // Loads the Npgsql provider so it generates Postgres SQL.
        var options = new DbContextOptionsBuilder<MudBarberDbContext>()
            .UseNpgsql("Host=localhost;Database=mudbarber;Username=postgres;Password=postgres")
            .Options;

        return new MudBarberDbContext(options);
    }
}