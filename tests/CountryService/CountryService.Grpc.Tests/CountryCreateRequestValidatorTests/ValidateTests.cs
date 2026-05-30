namespace CountryService.Grpc.Tests.CountryCreateRequestValidatorTests;

public sealed class ValidateTests
{
    private readonly CountryCreateRequestValidator _validator = new();

    [Fact]
    public void ShouldBeValid_WhenRequestIsValid()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ShouldHaveValidationError_WhenNameIsEmpty()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.Name = string.Empty;

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.ShouldHaveErrorFor(nameof(CountryCreateRequest.Name));
    }

    [Fact]
    public void ShouldHaveValidationError_WhenNameIsTooLong()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.Name = new string('a', 51);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.ShouldHaveErrorFor(nameof(CountryCreateRequest.Name));
    }

    [Fact]
    public void ShouldHaveValidationError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.Description = new string('a', 201);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.ShouldHaveErrorFor(nameof(CountryCreateRequest.Description));
    }

    [Fact]
    public void ShouldHaveValidationError_WhenFlagUriIsInvalid()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.FlagUri = "not-a-valid-uri";

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.ShouldHaveErrorFor(nameof(CountryCreateRequest.FlagUri));
    }

    [Fact]
    public void ShouldBeValid_WhenFlagUriIsEmpty()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.FlagUri = string.Empty;

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ShouldHaveValidationError_WhenLanguagesAreEmpty()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.Languages.Clear();

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.ShouldHaveErrorFor(nameof(CountryCreateRequest.Languages));
    }

    [Fact]
    public void ShouldHaveValidationError_WhenLanguagesContainEmptyItem()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.Languages.Add(0);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.Should().Contain(x => x.PropertyName.StartsWith(nameof(CountryCreateRequest.Languages)));
    }

    [Fact]
    public void ShouldHaveValidationError_WhenLanguagesContainDuplicates()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.Languages.AddRange([1, 1]);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.ShouldHaveErrorFor(nameof(CountryCreateRequest.Languages));
    }
}