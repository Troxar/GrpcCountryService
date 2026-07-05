using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CountryService.DAL.Tests.Infrastructure;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("CountryServiceTests")
        .WithUsername("postgres")
        .WithPassword("secretpassword")
        .Build();

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();
        await using var context = CreateContext();
        await context.Database.MigrateAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await using var context = CreateContext();
        await context.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE \"CountryLanguages\", \"Countries\" RESTART IDENTITY CASCADE;");
    }

    public CountryContext CreateContext(params IInterceptor[] interceptors)
    {
        var options = CreateContextOptions(interceptors);
        return new CountryContext(options);
    }

    public DbContextOptions<CountryContext> CreateContextOptions(params IInterceptor[] interceptors)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CountryContext>()
            .UseNpgsql(_postgres.GetConnectionString());

        if (interceptors.Length > 0)
            optionsBuilder.AddInterceptors(interceptors);

        return optionsBuilder.Options;
    }
}