namespace CountryService.BLL.Tests.CountryServiceTests;

public class DeleteTests : CountryServiceTestsBase
{
    [Fact]
    public async Task ShouldReturnTrue_WhenRepositoryReturnsAffectedRows()
    {
        // Arrange
        const int countryId = 1;
        CountryRepository.DeleteAsync(countryId).Returns(1);

        // Act
        var result = await CountryService.DeleteAsync(countryId);

        // Assert
        result.Should().BeTrue();
        await CountryRepository.Received(1).DeleteAsync(countryId);
    }

    [Fact]
    public async Task ShouldReturnFalse_WhenRepositoryReturnsZero()
    {
        // Arrange
        const int countryId = 999;
        CountryRepository.DeleteAsync(countryId).Returns(0);

        // Act
        var result = await CountryService.DeleteAsync(countryId);

        // Assert
        result.Should().BeFalse();
        await CountryRepository.Received(1).DeleteAsync(countryId);
    }
}