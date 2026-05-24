namespace CountryWiki.Web.Extensions;

using CountryServiceClient = CountryService.Grpc.v1.CountryService.CountryServiceClient;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCountryWikiWeb()
        {
            services.AddRazorPages();
            services.AddSingleton(new GlobalOptions { ProcessingUpload = false });

            return services;
        }

        public IServiceCollection AddCountryWikiServices()
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryService, BLL.Services.CountryService>();
            services.AddScoped<IFileUploadValidatorService, FileUploadValidatorService>();
            services.AddSingleton<ISyncCountriesChannel, SyncCountriesChannel>();
            services.AddHostedService<SyncUploadedCountriesBackgroundService>();
            services.AddTransient<TracerInterceptor>();

            return services;
        }

        public IServiceCollection AddCountryWikiGrpcClients(IConfiguration config)
        {
            var countryServiceUri = config.GetValue<string>("CountryServiceUri")
                                    ?? throw new InvalidOperationException("CountryServiceUri is not configured.");

            var grpcSection = config.GetSection(GrpcOptions.SectionName);
            services.Configure<GrpcOptions>(grpcSection);
            var grpcOptions = grpcSection.Get<GrpcOptions>() ?? new GrpcOptions();

            services.AddGrpcClient<CountryServiceClient>(options => { options.Address = new Uri(countryServiceUri); })
                .AddInterceptor<TracerInterceptor>()
                .ConfigureChannel(options => { options.ConfigureGrpcChannel(grpcOptions, grpcOptions.UseBrotliCompression); });

            services.AddGrpcClient<Health.HealthClient>(options => { options.Address = new Uri(countryServiceUri); })
                .ConfigureChannel(options => { options.ConfigureGrpcChannel(grpcOptions); });

            return services;
        }

        public IServiceCollection AddCountryWikiHealthChecks()
        {
            services.AddHealthChecks()
                .AddCheck("countrywiki-self", () => HealthCheckResult.Healthy(), ["live"])
                .AddCheck<CountryServiceGrpcHealthCheck>("countryservice-grpc", tags: ["ready"]);

            return services;
        }
    }
}