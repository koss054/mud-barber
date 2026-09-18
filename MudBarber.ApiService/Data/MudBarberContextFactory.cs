using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MudBarber.ApiService.Data;

public class MudBarberDbContextFactory : IDesignTimeDbContextFactory<MudBarberDbContext>
{
    // Enough to load the Npgsql provider, so `migrations add` works with no setup.
    // `database update` needs a real password, which comes from user secrets or
    // the ConnectionStrings__postgresdb env var. The port is pinned in AppHost.
    private const string FallbackConnectionString =
        "Host=localhost;Port=5432;Database=postgresdb;Username=postgres";

    public MudBarberDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<MudBarberDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var options = new DbContextOptionsBuilder<MudBarberDbContext>()
            .UseNpgsql(configuration.GetConnectionString("postgresdb") ?? FallbackConnectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new MudBarberDbContext(options);
    }
}
