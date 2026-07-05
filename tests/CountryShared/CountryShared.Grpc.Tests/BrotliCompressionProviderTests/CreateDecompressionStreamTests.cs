namespace CountryShared.Grpc.Tests.BrotliCompressionProviderTests;

public sealed class CreateDecompressionStreamTests
{
    private static readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task ShouldLeaveUnderlyingStreamOpenAfterDispose()
    {
        // Arrange
        var provider = new BrotliCompressionProvider();
        await using var compressedStream = new MemoryStream();
        var sourceBytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

        await using (var compressionStream = provider.CreateCompressionStream(compressedStream, null))
        {
            await compressionStream.WriteAsync(sourceBytes, CancellationToken);
        }

        compressedStream.Position = 0;

        // Act
        await using (var decompressionStream = provider.CreateDecompressionStream(compressedStream))
        {
            await decompressionStream.CopyToAsync(Stream.Null, CancellationToken);
        }

        // Assert
        compressedStream.CanRead.Should().BeTrue();
        compressedStream.Position = 0;
    }
}