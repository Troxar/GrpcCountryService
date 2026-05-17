namespace CountryWiki.BLL.Tests.FileUploadValidatorServiceTests;

public sealed class ParseFileTests
{
    private readonly FileUploadValidatorService _service = new();

    private static readonly JsonSerializerOptions UpperCaseJsonOptions = new()
    {
        PropertyNamingPolicy = new UpperCaseNamingPolicy()
    };

    [Fact]
    public async Task ShouldReturnCreateCountryModels_WhenJsonIsValid()
    {
        // Arrange
        var models = new[]
        {
            TestDataFactory.CreateCreateCountryModel(),
            TestDataFactory.CreateCreateCountryModel()
        };
        var json = JsonSerializer.Serialize(models);

        await using var stream = CreateStream(json);

        // Act
        var result = await _service.ParseFileAsync(stream);

        // Assert
        var resultModels = result.ToArray();
        resultModels.Should().BeEquivalentTo(models);
    }

    [Fact]
    public async Task ShouldDeserializeCaseInsensitively_WhenPropertyNamesHaveDifferentCase()
    {
        // Arrange
        var models = new[]
        {
            TestDataFactory.CreateCreateCountryModel()
        };
        var json = JsonSerializer.Serialize(models, UpperCaseJsonOptions);

        await using var stream = CreateStream(json);

        // Act
        var result = await _service.ParseFileAsync(stream);

        // Assert
        var country = result.Should().ContainSingle().Subject;
        country.Should().BeEquivalentTo(models.First());
    }

    [Theory]
    [InlineData("")]
    [InlineData("not valid json")]
    [InlineData("{ \"name\": \"France\" }")]
    public async Task ShouldReturnEmptyCollection_WhenJsonCannotBeParsedAsCountryCollection(string json)
    {
        // Arrange
        await using var stream = CreateStream(json);

        // Act
        var result = await _service.ParseFileAsync(stream);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnEmptyCollection_WhenJsonIsNullLiteral()
    {
        // Arrange
        await using var stream = CreateStream("null");

        // Act
        var result = await _service.ParseFileAsync(stream);

        // Assert
        result.Should().BeEmpty();
    }

    private static MemoryStream CreateStream(string content)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(content));
    }

    private class UpperCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToUpperInvariant();
        }
    }
}