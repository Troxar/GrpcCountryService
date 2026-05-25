extern alias CountryGrpcServer;
using CountryGrpcService = CountryGrpcServer::CountryService.Grpc.Services.CountryGrpcService;

namespace CountryWiki.Web.IntegrationTests.Infrastructure;

public sealed class CountryGrpcServiceFactory : WebApplicationFactory<CountryGrpcService>
{
    private readonly string _connectionString;

    public CountryGrpcServiceFactory(string connectionString)
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
                ["ConnectionStrings:CountryService"] = _connectionString,
                ["Database:ApplyMigrationsOnStartup"] = "false"
            });
        });
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<CountryContext>>();
            services.RemoveAll<CountryContext>();
            services.AddDbContext<CountryContext>(options => options.UseNpgsql(_connectionString));
        });
    }
}