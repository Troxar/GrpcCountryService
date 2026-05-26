namespace CountryWiki.Web.Tests.EditModelTests;

public sealed class OnGetTests : EditModelTestsBase
{
    [Fact]
    public async Task ShouldReturnPageAndFillModel_WhenCountryExists()
    {
        // Arrange
        var country = TestDataFactory.CreateCountryModel(1);
        CountryService.GetAsync(country.Id, Arg.Any<CancellationToken>()).Returns(country);

        // Act
        var result = await EditModel.OnGetAsync(country.Id, CancellationToken);

        // Assert
        result.Should().BeOfType<PageResult>();
        EditModel.CountryName.Should().Be(country.Name);
        EditModel.CountryToUpdate.Should().BeEquivalentTo(country, options => options.ExcludingMissingMembers());

        await CountryService.Received(1).GetAsync(country.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenCountryDoesNotExist()
    {
        // Arrange
        const int countryId = 999;
        CountryService.GetAsync(countryId, Arg.Any<CancellationToken>()).Returns((CountryModel?)null);

        // Act
        var result = await EditModel.OnGetAsync(countryId, CancellationToken);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        EditModel.CountryName.Should().BeEmpty();
        EditModel.CountryToUpdate.Id.Should().Be(0);
        EditModel.CountryToUpdate.Description.Should().BeNullOrEmpty();

        await CountryService.Received(1).GetAsync(countryId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldRedirectToError_WhenCountryServiceFails()
    {
        // Arrange
        const int countryId = 1;
        var exceptionMessage = Guid.NewGuid().ToString();
        CountryService.GetAsync(countryId, Arg.Any<CancellationToken>()).Returns<CountryModel?>(_ =>
            throw new CountryServiceException(CountryServiceErrorCode.ServiceUnavailable, exceptionMessage));

        // Act
        var result = await EditModel.OnGetAsync(countryId, CancellationToken);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("/Error");
        redirectResult.RouteValues.Should().ContainKey("message").WhoseValue.Should().Be(exceptionMessage);

        await CountryService.Received(1).GetAsync(countryId, Arg.Any<CancellationToken>());
    }
}