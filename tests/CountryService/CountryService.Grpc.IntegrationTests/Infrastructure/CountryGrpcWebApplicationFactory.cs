namespace CountryService.Grpc.IntegrationTests.Infrastructure;

public sealed class CountryGrpcWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public CountryGrpcWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:CountryService"] = _connectionString
            });
        });
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<CountryContext>>();
            services.RemoveAll<CountryContext>();
            services.AddDbContext<CountryContext>(options => { options.UseNpgsql(_connectionString); });
        });
    }
}