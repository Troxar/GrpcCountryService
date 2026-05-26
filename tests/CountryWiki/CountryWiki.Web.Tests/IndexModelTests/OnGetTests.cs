namespace CountryWiki.Web.Tests.IndexModelTests;

public sealed class OnGetTests : IndexModelTestsBase
{
    [Fact]
    public async Task ShouldLoadCountries_WhenCountryServiceSucceeds()
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
    public async Task ShouldSetErrorMessageAndEmptyCountries_WhenCountryServiceThrowsServiceUnavailable()
    {
        // Arrange
        var exception = TestDataFactory.CreateCountryServiceException(CountryServiceErrorCode.ServiceUnavailable);
        CountryService.GetAllAsync().Returns(Task.FromException<IEnumerable<CountryModel>>(exception));

        // Act
        await IndexModel.OnGetAsync();

        // Assert
        IndexModel.Countries.Should().BeEmpty();
        IndexModel.ErrorMessage.Should().Be(exception.Message);
    }
}