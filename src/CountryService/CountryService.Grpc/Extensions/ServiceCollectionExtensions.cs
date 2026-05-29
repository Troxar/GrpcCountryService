namespace CountryService.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCountryServiceGrpc(IConfiguration config, IWebHostEnvironment env)
        {
            var grpcSection = config.GetSection(GrpcOptions.SectionName);
            services.Configure<GrpcOptions>(grpcSection);
            var grpcOptions = grpcSection.Get<GrpcOptions>() ?? new GrpcOptions();

            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = env.IsDevelopment();
                options.IgnoreUnknownServices = true;

                options.MaxReceiveMessageSize = grpcOptions.MaxReceiveMessageSize;
                options.MaxSendMessageSize = grpcOptions.MaxSendMessageSize;

                options.CompressionProviders = new List<ICompressionProvider>
                {
                    new BrotliCompressionProvider()
                };
                options.ResponseCompressionAlgorithm = "br";
                options.ResponseCompressionLevel = CompressionLevel.Optimal;

                options.Interceptors.Add<ExceptionInterceptor>();
            });

            if (env.IsDevelopment())
                services.AddGrpcReflection();

            return services;
        }

        public IServiceCollection AddCountryServices()
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryService, BLL.Services.CountryService>();

            return services;
        }

        public IServiceCollection AddCountryServiceHealthChecks()
        {
            services.AddGrpcHealthChecks(options =>
                {
                    options.Services.Map(string.Empty, context => context.Tags.Contains("ready"));
                    options.Services.Map(v1.CountryService.Descriptor.FullName,
                        context => context.Tags.Contains("ready"));
                })
                .AddCheck("countryservice-self", () => HealthCheckResult.Healthy(), ["live"])
                .AddDbContextCheck<CountryContext>("countryservice-db", tags: ["ready"]);

            return services;
        }

        public IServiceCollection AddCountryServiceDatabase(IConfiguration config)
        {
            services.Configure<DatabaseOptions>(config.GetSection(DatabaseOptions.SectionName));
            services.AddDbContext<CountryContext>();

            return services;
        }

        public IServiceCollection AddCountryServiceValidation()
        {
            services.AddValidatorsFromAssemblyContaining<CountryCreateRequestValidator>();

            return services;
        }
    }
}