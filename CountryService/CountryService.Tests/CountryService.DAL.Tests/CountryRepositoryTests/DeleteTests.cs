namespace CountryService.DAL.Tests.CountryRepositoryTests;

public class DeleteTests(PostgreSqlFixture fixture) : CountryRepositoryTestsBase(fixture)
{
    [Fact]
    public async Task ShouldDeleteCountry_AndReturnAffectedRows_WhenCountryExists()
    {
        // Arrange
        await using var context = Fixture.CreateContext();

        var repository = new CountryRepository(context);
        var country = TestDataFactory.CreateCountry();

        await context.Countries.AddAsync(country, CancellationToken);
        await context.SaveChangesAsync(CancellationToken);

        // Act
        var affectedRows = await repository.DeleteAsync(country.Id);

        // Assert
        affectedRows.Should().Be(1);

        var exists = await context.Countries
            .AsNoTracking()
            .AnyAsync(x => x.Id == country.Id, CancellationToken);
        exists.Should().BeFalse();
    }
    
    [Fact]
    public async Task ShouldReturnZero_WhenCountryDoesNotExist()
    {
        // Arrange
        await using var context = Fixture.CreateContext();
        
        var repository = new CountryRepository(context);

        // Act
        var affectedRows = await repository.DeleteAsync(999);

        // Assert
        affectedRows.Should().Be(0);
    }
}