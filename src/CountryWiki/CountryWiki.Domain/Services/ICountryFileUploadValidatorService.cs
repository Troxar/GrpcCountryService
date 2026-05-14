namespace CountryWiki.Domain.Services;

public interface IFileUploadValidatorService
{
    bool ValidateFile(UploadedFileModel uploadedFile);
    Task<IEnumerable<CreateCountryModel>> ParseFileAsync(Stream content);
}