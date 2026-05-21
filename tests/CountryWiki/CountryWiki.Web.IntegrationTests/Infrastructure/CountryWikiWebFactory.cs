using CountryServiceClient = CountryService.Grpc.v1.CountryService.CountryServiceClient;

namespace CountryWiki.Web.IntegrationTests.Infrastructure;

public sealed class CountryWikiWebFactory : WebApplicationFactory<IndexModel>
{
    private readonly HttpMessageHandler _countryServiceHandler;

    public CountryWikiWebFactory(HttpMessageHandler countryServiceHandler)
    {
        _countryServiceHandler = countryServiceHandler;
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
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<CountryServiceClient>();
            services.AddGrpcClient<CountryServiceClient>(options => options.Address = new Uri("https://localhost"))
                .ConfigurePrimaryHttpMessageHandler(() => _countryServiceHandler)
                .ConfigureChannel(options =>
                {
                    options.CompressionProviders = new List<ICompressionProvider>
                    {
                        new BrotliCompressionProvider()
                    };
                    options.MaxReceiveMessageSize = 1024 * 1024 * 6;
                    options.MaxSendMessageSize = 1024 * 1024 * 6;
                });
        });
    }
}