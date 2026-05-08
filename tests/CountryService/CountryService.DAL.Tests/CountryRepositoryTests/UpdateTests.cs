namespace CountryService.DAL.Tests.CountryRepositoryTests;

[Collection(PostgreSqlCollection.Name)]
public class UpdateTests(PostgreSqlFixture fixture) : CountryRepositoryTestsBase(fixture)
{
    [Fact]
    public async Task ShouldUpdateCountry_AndReturnAffectedRows_WhenCountryExists()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var repository = new CountryRepository(context);
        var country = TestDataFactory.CreateCountry();

        await context.Countries.AddAsync(country, CancellationToken);
        await context.SaveChangesAsync(CancellationToken);

        var model = TestDataFactory.UpdateCountryModel(country.Id);

        // Act
        var affectedRows = await repository.UpdateAsync(model);

        // Assert
        affectedRows.Should().Be(1);

        var updatedCountry = await context.Countries
            .AsNoTracking()
            .SingleAsync(x => x.Id == country.Id, CancellationToken);

        updatedCountry.Description.Should().Be(model.Description);
        updatedCountry.UpdateDate.Should().BeCloseTo(model.UpdateDate, TimeSpan.FromMilliseconds(1));
    }

    [Fact]
    public async Task ShouldReturnZero_WhenCountryDoesNotExist()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var repository = new CountryRepository(context);
        var model = TestDataFactory.UpdateCountryModel(999);

        // Act
        var affectedRows = await repository.UpdateAsync(model);

        // Assert
        affectedRows.Should().Be(0);
    }
}