var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCountryServiceGrpc(builder.Configuration, builder.Environment)
    .AddDbContext<CountryContext>()
    .AddCountryServices()
    .AddCountryServiceHealthChecks();

var app = builder.Build();

app.ApplyMigrations();
app.MapCountryServiceEndpoints();

app.Run();