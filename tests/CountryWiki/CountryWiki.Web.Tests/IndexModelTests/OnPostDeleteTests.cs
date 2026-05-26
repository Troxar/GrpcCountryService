namespace CountryWiki.Web.Tests.IndexModelTests;

public sealed class OnPostDeleteTests : IndexModelTestsBase
{
    [Fact]
    public async Task ShouldDeleteCountry_AndRedirectToIndex()
    {
        // Arrange
        const int countryId = 1;
        CountryService.DeleteAsync(countryId).Returns(Task.CompletedTask);

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("./Index");

        await CountryService.Received(1).DeleteAsync(countryId);
    }

    [Fact]
    public async Task ShouldReturnPageWithError_WhenCountryServiceFails()
    {
        // Arrange
        const int countryId = 1;
        var countries = new[]
        {
            TestDataFactory.CreateCountryModel(1)
        };
        var message = Guid.NewGuid().ToString();
        CountryService.DeleteAsync(countryId).Returns(_ =>
            throw new CountryServiceException(CountryServiceErrorCode.ServiceUnavailable, message));
        CountryService.GetAllAsync().Returns(countries);

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId);

        // Assert
        result.Should().BeOfType<PageResult>();
        IndexModel.ErrorMessage.Should().Be(message);
        IndexModel.Countries.Should().BeEquivalentTo(countries);

        await CountryService.Received(1).DeleteAsync(countryId);
        await CountryService.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task ShouldKeepDeleteError_WhenReloadingCountriesFails()
    {
        // Arrange
        const int countryId = 1;
        var deleteExceptionMessage = Guid.NewGuid().ToString();
        CountryService.DeleteAsync(countryId).Returns(_ =>
            throw new CountryServiceException(CountryServiceErrorCode.ServiceUnavailable, deleteExceptionMessage));
        CountryService.GetAllAsync().Returns<IEnumerable<CountryModel>>(_ =>
            throw new CountryServiceException(CountryServiceErrorCode.ServiceUnavailable, Guid.NewGuid().ToString()));

        // Act
        var result = await IndexModel.OnPostDeleteAsync(countryId);

        // Assert
        result.Should().BeOfType<PageResult>();
        IndexModel.ErrorMessage.Should().Be(deleteExceptionMessage);
        IndexModel.Countries.Should().BeEmpty();

        await CountryService.Received(1).DeleteAsync(countryId);
        await CountryService.Received(1).GetAllAsync();
    }
}