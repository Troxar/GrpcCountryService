var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.IgnoreUnknownServices = true;
    options.MaxReceiveMessageSize = 1024 * 1024 * 6;
    options.MaxSendMessageSize = 1024 * 1024 * 6;
    options.CompressionProviders = new List<ICompressionProvider>
    {
        new BrotliCompressionProvider()
    };
    options.ResponseCompressionAlgorithm = "br";
    options.ResponseCompressionLevel = CompressionLevel.Optimal;
    options.Interceptors.Add<ExceptionInterceptor>();
});
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryService.BLL.Services.CountryService>();
builder.Services.AddDbContext<CountryContext>();
builder.Services.AddGrpcHealthChecks(options =>
    {
        options.Services.Map(string.Empty, registration => registration.Tags.Contains("ready"));
        options.Services.Map(CountryService.Grpc.v1.CountryService.Descriptor.FullName,
            registration => registration.Tags.Contains("ready"));
    })
    .AddCheck("countryservice-self", () => HealthCheckResult.Healthy(), ["live"])
    .AddDbContextCheck<CountryContext>("countryservice-db", tags: ["ready"]);

if (builder.Environment.IsDevelopment())
    builder.Services.AddGrpcReflection();

var app = builder.Build();

app.ApplyMigrations();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client");
app.MapGrpcService<CountryGrpcService>();
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains("live")
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains("ready")
});
app.MapGrpcHealthChecksService();

if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();

app.Run();