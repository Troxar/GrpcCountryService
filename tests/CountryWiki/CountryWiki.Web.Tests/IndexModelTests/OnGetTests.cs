namespace CountryWiki.Web.Tests.IndexModelTests;

public sealed class OnGetTests : IndexModelTestsBase
{
    [Fact]
    public async Task ShouldLoadCountries()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCountryModel(1),
            TestDataFactory.CreateCountryModel(2)
        };
        CountryService.GetAllAsync().Returns(countries);

        // Act
        await IndexModel.OnGetAsync();

        // Assert
        IndexModel.Countries.Should().BeEquivalentTo(countries);
        await CountryService.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task ShouldSetErrorMessage_WhenCountryServiceFails()
    {
        // Arrange
        var exceptionMessage = Guid.NewGuid().ToString();
        CountryService.GetAllAsync().Returns<IEnumerable<CountryModel>>(_ =>
            throw new CountryServiceException(CountryServiceErrorCode.ServiceUnavailable, exceptionMessage));

        // Act
        await IndexModel.OnGetAsync();

        // Assert
        IndexModel.ErrorMessage.Should().Be(exceptionMessage);
        IndexModel.Countries.Should().BeEmpty();
        await CountryService.Received(1).GetAllAsync();
    }
}