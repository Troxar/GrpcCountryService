namespace CountryService.Grpc.IntegrationTests.IntegrationTests;

[Collection(CountryGrpcIntegrationCollection.Name)]
public abstract class IntegrationTestsBase : IAsyncLifetime
{
    protected readonly CountryGrpcIntegrationFixture Fixture;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected IntegrationTestsBase(CountryGrpcIntegrationFixture fixture)
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
        await using var scope = Fixture.Factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CountryContext>();

        await context.Countries.AddRangeAsync(countries, CancellationToken);
        await context.SaveChangesAsync(CancellationToken);
    }

    protected async Task<Country?> FindCountryAsync(int countryId)
    {
        await using var scope = Fixture.Factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CountryContext>();

        return await context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == countryId, CancellationToken);
    }

    protected async Task<int> CountCountriesAsync()
    {
        await using var scope = Fixture.Factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CountryContext>();

        return await context.Countries.CountAsync(CancellationToken);
    }
}