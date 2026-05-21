namespace CountryWiki.Web.IntegrationTests.Infrastructure;

public class CountryWikiIntegrationFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("CountryWikiIntegrationTests")
        .WithUsername("postgres")
        .WithPassword("secretpassword")
        .Build();

    private CountryWikiWebFactory _countryWikiWebFactory = null!;

    public CountryGrpcServiceFactory CountryGrpcServiceFactory { get; private set; } = null!;
    public HttpClient HttpClient { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();

        CountryGrpcServiceFactory = new CountryGrpcServiceFactory(_postgres.GetConnectionString());

        await using (var scope = CountryGrpcServiceFactory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CountryContext>();
            await context.Database.MigrateAsync();
        }

        _countryWikiWebFactory = new CountryWikiWebFactory(CountryGrpcServiceFactory.Server.CreateHandler());
        HttpClient = _countryWikiWebFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    public async ValueTask DisposeAsync()
    {
        HttpClient.Dispose();

        await _countryWikiWebFactory.DisposeAsync();
        await CountryGrpcServiceFactory.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await using var scope = CountryGrpcServiceFactory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CountryContext>();

        await context.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE \"CountryLanguages\", \"Countries\" RESTART IDENTITY CASCADE;");
    }
}