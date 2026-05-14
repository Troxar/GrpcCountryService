namespace CountryWiki.Domain.Models;

public record UploadedFileModel
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
}