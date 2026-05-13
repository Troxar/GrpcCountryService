namespace CountryService.Grpc.IntegrationTests.Infrastructure;

public class CountryGrpcIntegrationFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("CountryServiceGrpcTests")
        .WithUsername("postgres")
        .WithPassword("secretpassword")
        .Build();

    public CountryGrpcWebApplicationFactory Factory { get; private set; } = null!;
    public v1.CountryService.CountryServiceClient Client { get; private set; } = null!;
    public ResponseHeadersCaptureHandler ResponseHeadersCaptureHandler { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();

        Factory = new CountryGrpcWebApplicationFactory(_postgres.GetConnectionString());

        await using var scope = Factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CountryContext>();

        await context.Database.MigrateAsync();

        ResponseHeadersCaptureHandler = new ResponseHeadersCaptureHandler(Factory.Server.CreateHandler());
        var channel = GrpcChannel.ForAddress("https://localhost", new GrpcChannelOptions
        {
            HttpHandler = ResponseHeadersCaptureHandler,
            CompressionProviders = new List<ICompressionProvider>
            {
                new BrotliCompressionProvider()
            }
        });
        Client = new v1.CountryService.CountryServiceClient(channel);
    }

    public async ValueTask DisposeAsync()
    {
        await Factory.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await using var scope = Factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CountryContext>();

        await context.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE \"CountryLanguages\", \"Countries\" RESTART IDENTITY CASCADE;");
    }
}