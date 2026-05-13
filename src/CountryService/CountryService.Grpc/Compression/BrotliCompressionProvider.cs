namespace CountryService.Grpc.Compression;

public class BrotliCompressionProvider : ICompressionProvider
{
    private readonly CompressionLevel? _compressionLevel;

    public string EncodingName => "br";

    public BrotliCompressionProvider(CompressionLevel compressionLevel)
    {
        _compressionLevel = compressionLevel;
    }

    public BrotliCompressionProvider()
    {
    }

    public Stream CreateCompressionStream(Stream stream, CompressionLevel? compressionLevel)
    {
        var level = compressionLevel ?? _compressionLevel ?? CompressionLevel.Optimal;
        return new BrotliStream(stream, level, true);
    }

    public Stream CreateDecompressionStream(Stream stream)
    {
        return new BrotliStream(stream, CompressionMode.Decompress, true);
    }
}