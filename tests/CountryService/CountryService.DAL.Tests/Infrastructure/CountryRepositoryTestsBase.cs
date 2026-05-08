namespace CountryService.DAL.Tests.Infrastructure;

[Collection(PostgreSqlCollection.Name)]
public abstract class CountryRepositoryTestsBase(PostgreSqlFixture fixture) : IAsyncLifetime
{
    protected readonly PostgreSqlFixture Fixture = fixture;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    public async ValueTask InitializeAsync()
    {
        await Fixture.ResetDatabaseAsync();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}