namespace CountryService.BLL.Tests.CountryServiceTests;

public sealed class UpdateTests : CountryServiceTestsBase
{
    [Fact]
    public async Task ShouldReturnTrue_WhenRepositoryReturnsAffectedRows()
    {
        // Arrange
        var model = TestDataFactory.CreateUpdateCountryModel(1);
        CountryRepository.UpdateAsync(model, Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await CountryService.UpdateAsync(model, CancellationToken);

        // Assert
        result.Should().BeTrue();
        await CountryRepository.Received(1).UpdateAsync(model, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldReturnFalse_WhenRepositoryReturnsZero()
    {
        // Arrange
        var model = TestDataFactory.CreateUpdateCountryModel(999);
        CountryRepository.UpdateAsync(model, Arg.Any<CancellationToken>()).Returns(0);

        // Act
        var result = await CountryService.UpdateAsync(model, CancellationToken);

        // Assert
        result.Should().BeFalse();
        await CountryRepository.Received(1).UpdateAsync(model, Arg.Any<CancellationToken>());
    }
}