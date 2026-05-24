using CountryServiceClient = CountryService.Grpc.v1.CountryService.CountryServiceClient;

namespace CountryWiki.Web.IntegrationTests.Infrastructure;

public sealed class CountryWikiWebFactory : WebApplicationFactory<IndexModel>
{
    private readonly HttpMessageHandler _countryServiceHandler;
    private readonly TestLoggerProvider _loggerProvider;

    public CountryWikiWebFactory(HttpMessageHandler countryServiceHandler, TestLoggerProvider loggerProvider)
    {
        _countryServiceHandler = countryServiceHandler;
        _loggerProvider = loggerProvider;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["CountryServiceUri"] = "http://localhost"
            });
        });
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddFilter(typeof(TracerInterceptor).FullName, LogLevel.Debug);
            logging.AddProvider(_loggerProvider);
        });
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<CountryServiceClient>();
            services.RemoveAll<Health.HealthClient>();
            services.AddGrpcClient<CountryServiceClient>(options => { options.Address = new Uri("http://localhost"); })
                .ConfigurePrimaryHttpMessageHandler(() => _countryServiceHandler)
                .AddInterceptor<TracerInterceptor>()
                .ConfigureChannel(options => { options.ConfigureGrpcChannel(useBrotliCompression: true); });

            services.AddGrpcClient<Health.HealthClient>(options => { options.Address = new Uri("http://localhost"); })
                .ConfigurePrimaryHttpMessageHandler(() => _countryServiceHandler)
                .ConfigureChannel(options => { options.ConfigureGrpcChannel(); });
        });
    }
}