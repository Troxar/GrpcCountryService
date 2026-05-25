var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCountryServiceGrpc(builder.Configuration, builder.Environment)
    .AddCountryServiceDatabase(builder.Configuration)
    .AddCountryServices()
    .AddCountryServiceHealthChecks();

var app = builder.Build();

await app.ApplyMigrationsIfConfiguredAsync();
app.MapCountryServiceEndpoints();

await app.RunAsync();