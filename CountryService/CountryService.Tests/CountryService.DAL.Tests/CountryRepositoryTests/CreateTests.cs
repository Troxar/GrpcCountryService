namespace CountryService.DAL.Tests.CountryRepositoryTests;

public class CreateTests(PostgreSqlFixture fixture) : CountryRepositoryTestsBase(fixture)
{
    [Fact]
    public async Task ShouldCreateCountry_AndReturnGeneratedId()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var repository = new CountryRepository(context);
        var model = TestDataFactory.CreateCountryModel(1, 2);

        // Act
        var id = await repository.CreateAsync(model);

        // Assert
        id.Should().BeGreaterThan(0);

        var savedCountry = await context.Countries
            .AsNoTracking()
            .Include(x => x.CountryLanguages)
            .SingleAsync(x => x.Id == id, CancellationToken);

        savedCountry.Name.Should().Be(model.Name);
        savedCountry.Description.Should().Be(model.Description);
        savedCountry.FlagUri.Should().Be(model.FlagUri);
        savedCountry.CapitalCity.Should().Be(model.CapitalCity);
        savedCountry.Anthem.Should().Be(model.Anthem);
        savedCountry.CreateDate.Should().NotBe(default);
        savedCountry.UpdateDate.Should().BeNull();

        savedCountry.CountryLanguages.Should().NotBeNullOrEmpty();
        savedCountry.CountryLanguages.Select(x => x.LanguageId).Should().BeEquivalentTo(model.Languages);
    }
}