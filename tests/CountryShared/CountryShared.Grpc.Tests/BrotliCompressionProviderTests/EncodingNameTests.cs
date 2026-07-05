namespace CountryShared.Grpc.Tests.BrotliCompressionProviderTests;

public sealed class EncodingNameTests
{
    [Fact]
    public void ShouldBeBr()
    {
        // Arrange
        var provider = new BrotliCompressionProvider();

        // Act
        var encodingName = provider.EncodingName;

        // Assert
        encodingName.Should().Be("br");
    }
}