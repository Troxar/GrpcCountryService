namespace CountryWiki.Web.Tests.EditModelTests;

public sealed class OnPostTests : EditModelTestsBase
{
    [Fact]
    public async Task ShouldUpdateCountry_AndRedirectToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var countryToUpdate = TestDataFactory.CreateUpdateCountry(1);
        EditModel.CountryToUpdate = countryToUpdate;
        CountryService.UpdateAsync(Arg.Any<UpdateCountryModel>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await EditModel.OnPostAsync(CancellationToken);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("./Index");

        await CountryService.Received(1).UpdateAsync(Arg.Is<UpdateCountryModel>(x =>
            x.Id == countryToUpdate.Id
            && x.Description == countryToUpdate.Description), Arg.Any<CancellationToken>());
        await CountryService.DidNotReceive().GetAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldReloadCountry_AndReturnPage_WhenModelStateIsInvalid()
    {
        // Arrange
        var country = TestDataFactory.CreateCountryModel(1);
        var countryToUpdate = TestDataFactory.CreateUpdateCountry(country.Id);
        EditModel.CountryToUpdate = countryToUpdate;
        const string propertyName = nameof(UpdateCountry.Description);
        EditModel.ModelState.AddModelError(propertyName, $"{propertyName} is required");
        CountryService.GetAsync(country.Id, Arg.Any<CancellationToken>()).Returns(country);

        // Act
        var result = await EditModel.OnPostAsync(CancellationToken);

        // Assert
        result.Should().BeOfType<PageResult>();

        EditModel.CountryName.Should().Be(country.Name);
        EditModel.CountryToUpdate.Should().BeEquivalentTo(countryToUpdate);

        await CountryService.Received(1).GetAsync(country.Id, Arg.Any<CancellationToken>());
        await CountryService.DidNotReceive().UpdateAsync(Arg.Any<UpdateCountryModel>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldReturnPageWithoutUpdate_WhenModelStateIsInvalid_AndCountryDoesNotExist()
    {
        // Arrange
        var countryToUpdate = TestDataFactory.CreateUpdateCountry(999);
        EditModel.CountryToUpdate = countryToUpdate;
        const string propertyName = nameof(UpdateCountry.Description);
        EditModel.ModelState.AddModelError(propertyName, $"{propertyName} is required");
        CountryService.GetAsync(countryToUpdate.Id, Arg.Any<CancellationToken>()).Returns((CountryModel?)null);

        // Act
        var result = await EditModel.OnPostAsync(CancellationToken);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task ShouldRedirectToErrorPage_WhenModelStateIsInvalid_AndCountryServiceFails()
    {
        // Arrange
        var countryToUpdate = TestDataFactory.CreateUpdateCountry(1);
        EditModel.CountryToUpdate = countryToUpdate;
        const string propertyName = nameof(UpdateCountry.Description);
        EditModel.ModelState.AddModelError(propertyName, $"{propertyName} is required");
        var exceptionMessage = Guid.NewGuid().ToString();
        CountryService.GetAsync(countryToUpdate.Id, Arg.Any<CancellationToken>()).Returns<CountryModel?>(_ =>
            throw new CountryServiceException(CountryServiceErrorCode.ServiceUnavailable, exceptionMessage));

        // Act
        var result = await EditModel.OnPostAsync(CancellationToken);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("/Error");
        redirectResult.RouteValues.Should().ContainKey("message").WhoseValue.Should().Be(exceptionMessage);
        EditModel.CountryToUpdate.Should().BeEquivalentTo(countryToUpdate);

        await CountryService.Received(1).GetAsync(countryToUpdate.Id, Arg.Any<CancellationToken>());
        await CountryService.DidNotReceive().UpdateAsync(Arg.Any<UpdateCountryModel>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldRedirectToErrorPage_WhenUpdateThrowsTimeout()
    {
        // Arrange
        EditModel.CountryToUpdate = TestDataFactory.CreateUpdateCountry(10);
        var exception = TestDataFactory.CreateCountryServiceException(CountryServiceErrorCode.Timeout);
        CountryService.UpdateAsync(Arg.Any<UpdateCountryModel>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(exception));

        // Act
        var result = await EditModel.OnPostAsync(CancellationToken);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("/Error");
    }
}