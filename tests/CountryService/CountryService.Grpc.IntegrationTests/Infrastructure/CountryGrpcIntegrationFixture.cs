namespace CountryService.Grpc.IntegrationTests.Infrastructure;

public class CountryGrpcIntegrationFixture : IAsyncLifetime
{
    public readonly PostgreSqlContainer Postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("CountryServiceGrpcTests")
        .WithUsername("postgres")
        .WithPassword("secretpassword")
        .Build();

    public CountryGrpcWebApplicationFactory Factory { get; private set; } = null!;
    public v1.CountryService.CountryServiceClient Client { get; private set; } = null!;
    public ResponseHeadersCaptureHandler ResponseHeadersCaptureHandler { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await Postgres.StartAsync();

        Factory = new CountryGrpcWebApplicationFactory(Postgres.GetConnectionString());

        await using var scope = Factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CountryContext>();

        await context.Database.MigrateAsync();

        ResponseHeadersCaptureHandler = new ResponseHeadersCaptureHandler(Factory.Server.CreateHandler());
        var channel = GrpcChannel.ForAddress("https://localhost", new GrpcChannelOptions
        {
            HttpHandler = ResponseHeadersCaptureHandler
        }.ConfigureGrpcChannel(useBrotliCompression: true));
        Client = new v1.CountryService.CountryServiceClient(channel);
    }

    public async ValueTask DisposeAsync()
    {
        await Factory.DisposeAsync();
        await Postgres.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await using var scope = Factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CountryContext>();

        await context.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE \"CountryLanguages\", \"Countries\" RESTART IDENTITY CASCADE;");
    }
}