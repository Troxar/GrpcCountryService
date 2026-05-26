namespace CountryWiki.BLL.Services;

public class FileUploadValidatorService : IFileUploadValidatorService
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public bool ValidateFile(UploadedFileModel uploadedFile)
    {
        return uploadedFile.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
               && uploadedFile.ContentType.StartsWith(MediaTypeNames.Application.Json,
                   StringComparison.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<CreateCountryModel>> ParseFileAsync(Stream content,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<CreateCountryModel>>(content, Options,
                cancellationToken) ?? [];
        }
        catch
        {
            return [];
        }
    }
}