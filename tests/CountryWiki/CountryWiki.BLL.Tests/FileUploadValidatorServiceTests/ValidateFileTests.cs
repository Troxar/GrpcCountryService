namespace CountryWiki.BLL.Tests.FileUploadValidatorServiceTests;

public sealed class ValidateFileTests
{
    private readonly FileUploadValidatorService _service = new();

    [Theory]
    [InlineData("countries.json", MediaTypeNames.Application.Json)]
    [InlineData("countries.JSON", MediaTypeNames.Application.Json)]
    [InlineData("countries.json", $"{MediaTypeNames.Application.Json}; charset=utf-8")]
    public void ShouldReturnTrue_WhenExtensionOrContentTypeIsValid(string fileName, string contentType)
    {
        // Arrange
        var model = new UploadedFileModel
        {
            FileName = fileName,
            ContentType = contentType
        };

        // Act
        var result = _service.ValidateFile(model);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("countries.txt", "application/json")]
    [InlineData("countries.csv", "application/json")]
    [InlineData("countries", "application/json")]
    [InlineData("countries.json", "text/plain")]
    [InlineData("countries.json", "application/octet-stream")]
    public void ShouldReturnFalse_WhenExtensionOrContentTypeIsInvalid(string fileName, string contentType)
    {
        // Arrange
        var model = new UploadedFileModel
        {
            FileName = fileName,
            ContentType = contentType
        };

        // Act
        var result = _service.ValidateFile(model);

        // Assert
        result.Should().BeFalse();
    }
}