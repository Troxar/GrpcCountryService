namespace CountryWiki.Web.IntegrationTests.IntegrationTests;

public sealed class IndexPageTests(CountryWikiIntegrationFixture fixture) : IntegrationTestsBase(fixture)
{
    [Fact]
    public async Task ShouldRenderCountriesLoadedFromCountryServiceGrpc_WhenCountriesExist()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCountry(),
            TestDataFactory.CreateCountry()
        };
        await SeedCountriesAsync(countries);

        // Act
        var response = await Fixture.HttpClient.GetAsync("/", CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var html = await response.Content.ReadAsStringAsync(CancellationToken);
        foreach (var country in countries)
        {
            html.Should().Contain(country.Name);
            html.Should().Contain(country.Description);
        }
    }
}