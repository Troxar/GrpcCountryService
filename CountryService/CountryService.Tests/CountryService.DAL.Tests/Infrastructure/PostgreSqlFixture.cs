namespace CountryService.DAL.Tests.Infrastructure;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("CountryServiceTests")
        .WithUsername("postgres")
        .WithPassword("secretpassword")
        .Build();

    public string ConnectionString => _postgres.GetConnectionString();

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

    public CountryContext CreateContext()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:CountryService"] = ConnectionString
            })
            .Build();

        var options = new DbContextOptionsBuilder<CountryContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new CountryContext(options, configuration);
    }
}