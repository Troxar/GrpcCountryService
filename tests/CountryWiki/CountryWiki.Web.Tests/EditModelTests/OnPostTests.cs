namespace CountryWiki.Web.Tests.EditModelTests;

public sealed class OnPostTests : EditModelTestsBase
{
    [Fact]
    public async Task ShouldUpdateCountry_AndRedirectToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var countryToUpdate = TestDataFactory.CreateUpdateCountry(1);
        EditModel.CountryToUpdate = countryToUpdate;
        CountryService.UpdateAsync(Arg.Any<UpdateCountryModel>()).Returns(Task.CompletedTask);

        // Act
        var result = await EditModel.OnPostAsync();

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("./Index");

        await CountryService.Received(1).UpdateAsync(Arg.Is<UpdateCountryModel>(x =>
            x.Id == countryToUpdate.Id
            && x.Description == countryToUpdate.Description));
        await CountryService.DidNotReceive().GetAsync(Arg.Any<int>());
    }

    [Fact]
    public async Task ShouldReloadCountry_AndReturnPage_WhenModelStateIsInvalid()
    {
        // Arrange
        var country = TestDataFactory.CreateCountryModel(1);
        EditModel.CountryToUpdate = TestDataFactory.CreateUpdateCountry(country.Id);
        const string propertyName = nameof(UpdateCountry.Description);
        EditModel.ModelState.AddModelError(propertyName, $"{propertyName} is required");
        CountryService.GetAsync(country.Id).Returns(country);

        // Act
        var result = await EditModel.OnPostAsync();

        // Assert
        result.Should().BeOfType<PageResult>();

        EditModel.CountryName.Should().Be(country.Name);
        EditModel.CountryToUpdate.Should().BeEquivalentTo(country, options => options.ExcludingMissingMembers());

        await CountryService.Received(1).GetAsync(country.Id);
        await CountryService.DidNotReceive().UpdateAsync(Arg.Any<UpdateCountryModel>());
    }

    [Fact]
    public async Task ShouldReturnPageWithoutUpdate_WhenModelStateIsInvalid_AndCountryDoesNotExist()
    {
        // Arrange
        var countryToUpdate = TestDataFactory.CreateUpdateCountry(999);
        EditModel.CountryToUpdate = countryToUpdate;
        const string propertyName = nameof(UpdateCountry.Description);
        EditModel.ModelState.AddModelError(propertyName, $"{propertyName} is required");
        CountryService.GetAsync(countryToUpdate.Id).Returns((CountryModel?)null);

        // Act
        var result = await EditModel.OnPostAsync();

        // Assert
        result.Should().BeOfType<PageResult>();

        EditModel.CountryName.Should().BeEmpty();
        EditModel.CountryToUpdate.Should().BeEquivalentTo(countryToUpdate);

        await CountryService.Received(1).GetAsync(countryToUpdate.Id);
        await CountryService.DidNotReceive().UpdateAsync(Arg.Any<UpdateCountryModel>());
    }
}