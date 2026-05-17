namespace CountryService.Grpc.IntegrationTests.IntegrationTests;

public sealed class GetAllTests(CountryGrpcIntegrationFixture fixture) : IntegrationTestsBase(fixture)
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
        replies.Should().BeEquivalentTo(countries, options => options.ExcludingMissingMembers());
    }
}