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

    [Fact]
    public async Task ShouldLogGrpcCallThroughTracerInterceptor()
    {
        // Arrange
        var country = TestDataFactory.CreateCountry();
        await SeedCountriesAsync(country);

        // Act
        var response = await Fixture.HttpClient.GetAsync("/", CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Fixture.LoggerProvider.Records.Should().Contain(record =>
            record.CategoryName == typeof(TracerInterceptor).FullName
            && record.Level == LogLevel.Debug
            && record.Message.Contains(
                "Executing GetAll ServerStreaming method on service CountryService.v1.CountryService"));
    }
}