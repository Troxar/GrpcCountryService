namespace CountryService.BLL.Tests.CountryServiceTests;

public class UpdateTests : CountryServiceTestsBase
{
    [Fact]
    public async Task ShouldReturnTrue_WhenRepositoryReturnsAffectedRows()
    {
        // Arrange
        var model = TestDataFactory.CreateUpdateCountryModel(1);
        CountryRepository.UpdateAsync(model).Returns(1);

        // Act
        var result = await CountryService.UpdateAsync(model);

        // Assert
        result.Should().BeTrue();
        await CountryRepository.Received(1).UpdateAsync(model);
    }

    [Fact]
    public async Task ShouldReturnFalse_WhenRepositoryReturnsZero()
    {
        // Arrange
        var model = TestDataFactory.CreateUpdateCountryModel(999);
        CountryRepository.UpdateAsync(model).Returns(0);

        // Act
        var result = await CountryService.UpdateAsync(model);

        // Assert
        result.Should().BeFalse();
        await CountryRepository.Received(1).UpdateAsync(model);
    }
}