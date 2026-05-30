namespace CountryService.Grpc.Tests.CountryUpdateRequestValidatorTests;

public sealed class ValidateTests
{
    private readonly CountryUpdateRequestValidator _validator = new();

    [Fact]
    public void ShouldBeValid_WhenRequestIsValid()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryUpdateRequest(1);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ShouldHaveValidationError_WhenIdIsZero()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryUpdateRequest(0);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.ShouldHaveErrorFor(nameof(CountryUpdateRequest.Id));
    }

    [Fact]
    public void ShouldHaveValidationError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryUpdateRequest(0);
        request.Description = new string('a', 201);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.ShouldHaveErrorFor(nameof(CountryUpdateRequest.Description));
    }
}