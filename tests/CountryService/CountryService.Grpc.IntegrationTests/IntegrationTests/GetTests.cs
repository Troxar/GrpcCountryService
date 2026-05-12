namespace CountryService.Grpc.IntegrationTests.IntegrationTests;

public class GetTests(CountryGrpcIntegrationFixture fixture) : IntegrationTestsBase(fixture)
{
    [Fact]
    public async Task ShouldReturnCountryReply_WhenCountryExists()
    {
        // Arrange
        var country = TestDataFactory.CreateCountry();
        country.CountryLanguages = new List<CountryLanguage>
        {
            new() { LanguageId = 1 },
            new() { LanguageId = 2 }
        };
        await SeedCountriesAsync(country);

        var request = TestDataFactory.CreateCountryIdRequest(country.Id);

        // Act
        var reply = await Fixture.Client.GetAsync(request, cancellationToken: CancellationToken);

        // Assert
        reply.Id.Should().Be(country.Id);
        reply.Name.Should().Be(country.Name);
        reply.Description.Should().Be(country.Description);
        reply.FlagUri.Should().Be(country.FlagUri);
        reply.CapitalCity.Should().Be(country.CapitalCity);
        reply.Anthem.Should().Be(country.Anthem);
        reply.Languages.Should().BeEquivalentTo("English", "French");
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenCountryDoesNotExist()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryIdRequest(999);

        // Act
        var act = async () => await Fixture.Client.GetAsync(request, cancellationToken: CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Which.Status.Detail.Should().Contain("Country with Id 999 hasn't been found");
    }
}