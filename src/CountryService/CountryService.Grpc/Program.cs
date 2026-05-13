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

if (builder.Environment.IsDevelopment())
    builder.Services.AddGrpcReflection();

var app = builder.Build();

app.ApplyMigrations();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client");
app.MapGrpcService<CountryGrpcService>();

if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();

app.Run();