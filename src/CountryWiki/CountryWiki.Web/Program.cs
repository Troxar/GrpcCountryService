using CountryServiceClient = CountryService.Grpc.v1.CountryService.CountryServiceClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryWiki.BLL.Services.CountryService>();
builder.Services.AddScoped<IFileUploadValidatorService, FileUploadValidatorService>();
builder.Services.AddSingleton<ISyncCountriesChannel, SyncCountriesChannel>();
builder.Services.AddHostedService<SyncUploadedCountriesBackgroundService>();
builder.Services.AddSingleton(new GlobalOptions { ProcessingUpload = false });
builder.Services.AddTransient<TracerInterceptor>();
builder.Services.AddGrpcClient<CountryServiceClient>(options =>
    {
        var uri = builder.Configuration.GetValue<string>("CountryServiceUri")
                  ?? throw new InvalidOperationException("CountryServiceUri is not configured.");
        options.Address = new Uri(uri);
    })
    .AddInterceptor<TracerInterceptor>()
    .ConfigureChannel(options =>
    {
        options.CompressionProviders = new List<ICompressionProvider> { new BrotliCompressionProvider() };
        options.MaxReceiveMessageSize = 1024 * 1024 * 6;
        options.MaxSendMessageSize = 1024 * 1024 * 6;
    });
builder.Services.AddGrpcClient<Health.HealthClient>(options =>
    {
        var uri = builder.Configuration.GetValue<string>("CountryServiceUri")
                  ?? throw new InvalidOperationException("CountryServiceUri is not configured.");
        options.Address = new Uri(uri);
    })
    .ConfigureChannel(options =>
    {
        options.MaxReceiveMessageSize = 1024 * 1024 * 6;
        options.MaxSendMessageSize = 1024 * 1024 * 6;
    });
builder.Services.AddHealthChecks()
    .AddCheck("countrywiki-self", () => HealthCheckResult.Healthy(), ["live"])
    .AddCheck<CountryServiceGrpcHealthCheck>("countryservice-grpc", tags: ["ready"]);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains("live")
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains("ready")
});

app.Run();