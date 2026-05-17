namespace CountryService.DAL.Tests.CountryRepositoryTests;

public sealed class CreateTests(PostgreSqlFixture fixture) : CountryRepositoryTestsBase(fixture)
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

        var country = await context.Countries
            .AsNoTracking()
            .Include(x => x.CountryLanguages)
            .SingleAsync(x => x.Id == id, CancellationToken);

        country.Should().BeEquivalentTo(model, options => options
            .Excluding(x => x.Languages));
        country.CreateDate.Should().NotBe(default);
        country.UpdateDate.Should().BeNull();
        country.CountryLanguages.Should().NotBeNullOrEmpty();
        country.CountryLanguages.Select(x => x.LanguageId).Should().BeEquivalentTo(model.Languages);
    }
}