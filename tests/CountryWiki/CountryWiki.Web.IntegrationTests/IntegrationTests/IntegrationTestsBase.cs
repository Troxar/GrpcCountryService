namespace CountryWiki.Web.IntegrationTests.IntegrationTests;

[Collection(CountryWikiIntegrationCollection.Name)]
public abstract class IntegrationTestsBase : IAsyncLifetime
{
    protected readonly CountryWikiIntegrationFixture Fixture;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected IntegrationTestsBase(CountryWikiIntegrationFixture fixture)
    {
        Fixture = fixture;
    }

    public async ValueTask InitializeAsync()
    {
        await Fixture.ResetDatabaseAsync();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    protected async Task SeedCountriesAsync(params Country[] countries)
    {
        await using var scope = Fixture.CountryGrpcServiceFactory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CountryContext>();

        await context.Countries.AddRangeAsync(countries, CancellationToken);
        await context.SaveChangesAsync(CancellationToken);
    }
}