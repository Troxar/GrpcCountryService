namespace CountryWiki.Web.IntegrationTests.HealthChecksTests;

public abstract class HealthChecksTestsBase : IClassFixture<CountryWikiIntegrationFixture>
{
    protected readonly HttpClient HttpClient;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected HealthChecksTestsBase(CountryWikiIntegrationFixture fixture)
    {
        HttpClient = fixture.HttpClient;
    }
}