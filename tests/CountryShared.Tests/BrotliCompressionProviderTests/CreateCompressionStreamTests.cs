namespace CountryShared.Tests.BrotliCompressionProviderTests;

public sealed class CreateCompressionStreamTests
{
    private static readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task ShouldRoundTripPayload_AndCreateDecompressionStream()
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
            await compressionStream.WriteAsync(sourceBytes, CancellationToken);
        }

        compressedStream.Position = 0;

        await using var decompressedStream = new MemoryStream();
        await using (var decompressionStream = provider.CreateDecompressionStream(compressedStream))
        {
            await decompressionStream.CopyToAsync(decompressedStream, CancellationToken);
        }

        var decompressedText = Encoding.UTF8.GetString(decompressedStream.ToArray());

        // Assert
        compressedStream.Length.Should().BeGreaterThan(0);
        decompressedText.Should().Be(sourceText);
    }

    [Fact]
    public async Task ShouldLeaveUnderlyingStreamOpenAfterDispose()
    {
        // Arrange
        var provider = new BrotliCompressionProvider();
        await using var compressedStream = new MemoryStream();
        var sourceBytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

        // Act
        await using (var compressionStream = provider.CreateCompressionStream(compressedStream, null))
        {
            await compressionStream.WriteAsync(sourceBytes, CancellationToken);
        }

        // Assert
        compressedStream.CanRead.Should().BeTrue();
        compressedStream.CanWrite.Should().BeTrue();
        compressedStream.Position = 0;
        compressedStream.Length.Should().BeGreaterThan(0);
    }
}