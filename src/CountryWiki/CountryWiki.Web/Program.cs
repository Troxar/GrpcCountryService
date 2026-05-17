using CountryServiceClient = CountryService.Grpc.v1.CountryService.CountryServiceClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryWiki.BLL.Services.CountryService>();
builder.Services.AddScoped<IFileUploadValidatorService, FileUploadValidatorService>();
builder.Services.AddSingleton<ISyncCountriesChannel, SyncCountriesChannel>();
builder.Services.AddHostedService<SyncUploadedCountriesBackgroundService>();
builder.Services.AddSingleton(new GlobalOptions { ProcessingUpload = false });
builder.Services.AddGrpcClient<CountryServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration.GetSection("CountryServiceUri").Value ?? string.Empty);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();