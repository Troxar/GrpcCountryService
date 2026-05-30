namespace CountryService.Grpc.Tests.CountryIdRequestValidatorTests;

public sealed class ValidateTests
{
    private readonly CountryIdRequestValidator _validator = new();

    [Fact]
    public void ShouldBeValid_WhenIdIsGreaterThanZero()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryIdRequest(1);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ShouldHaveValidationError_WhenIdIsNotGreaterThanZero(int id)
    {
        // Arrange
        var request = TestDataFactory.CreateCountryIdRequest(id);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.ShouldHaveErrorFor(nameof(CountryIdRequest.Id));
    }
}