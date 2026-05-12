namespace CountryService.Grpc.IntegrationTests.IntegrationTests;

public class GetAllTests(CountryGrpcIntegrationFixture fixture) : IntegrationTestsBase(fixture)
{
    [Fact]
    public async Task ShouldReturnAllCountries_WhenCountriesExist()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCountry(),
            TestDataFactory.CreateCountry()
        };
        await SeedCountriesAsync(countries);

        // Act
        using var call = Fixture.Client.GetAll(new Empty(), cancellationToken: CancellationToken);

        var replies = new List<CountryReply>();
        await foreach (var reply in call.ResponseStream.ReadAllAsync(CancellationToken))
            replies.Add(reply);

        // Assert
        replies.Should().HaveCount(countries.Length);
        foreach (var country in countries)
            replies.Should().ContainSingle(x =>
                x.Id == country.Id
                && x.Name == country.Name
                && x.Description == country.Description);
    }
}