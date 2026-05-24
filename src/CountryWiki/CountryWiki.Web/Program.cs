var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCountryWikiWeb()
    .AddCountryWikiServices()
    .AddCountryWikiGrpcClients(builder.Configuration)
    .AddCountryWikiHealthChecks();

var app = builder.Build();

app.UseCountryWikiPipeline(app.Environment);
app.MapCountryWikiEndpoints();

app.Run();