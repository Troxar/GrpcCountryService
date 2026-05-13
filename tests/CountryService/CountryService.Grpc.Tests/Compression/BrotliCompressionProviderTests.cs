using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace CountryService.Grpc.Tests.Compression;

public class BrotliCompressionProviderTests
{
    [Fact]
    public async Task ShouldCompressAndDecompressPayload()
    {
        // Arrange
        var provider = new BrotliCompressionProvider(CompressionLevel.Optimal);
        var sourceText = string.Join('|', Enumerable.Repeat(Guid.NewGuid().ToString(), 100));
        var sourceBytes = Encoding.UTF8.GetBytes(sourceText);

        await using var compressedStream = new MemoryStream();

        // Act
        await using (var compressionStream =
                     provider.CreateCompressionStream(compressedStream, CompressionLevel.Optimal))
        {
            await compressionStream.WriteAsync(sourceBytes, TestContext.Current.CancellationToken);
        }

        compressedStream.Position = 0;

        await using var decompressedStream = new MemoryStream();
        await using (var decompressionStream = provider.CreateDecompressionStream(compressedStream))
        {
            await decompressionStream.CopyToAsync(decompressedStream, TestContext.Current.CancellationToken);
        }

        var decompressedText = Encoding.UTF8.GetString(decompressedStream.ToArray());

        // Assert
        provider.EncodingName.Should().Be("br");
        compressedStream.Length.Should().BeGreaterThan(0);
        decompressedText.Should().Be(sourceText);
    }
}