namespace CountryWiki.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ICountryService _countryService;
    private readonly IFileUploadValidatorService _countryFileUploadValidatorService;
    private readonly ISyncCountriesChannel _syncCountriesChannel;

    public GlobalOptions GlobalOptions { get; set; }
    public IEnumerable<CountryModel> Countries { get; set; } = new List<CountryModel>();
    public string ErrorMessage { get; set; } = string.Empty;

    [BindProperty]
    public IFormFile? Upload { get; set; }

    public IndexModel(ICountryService countryService, IFileUploadValidatorService countryFileUploadValidatorService,
        ISyncCountriesChannel syncCountriesChannel, GlobalOptions globalOptions)
    {
        _countryService = countryService;
        _countryFileUploadValidatorService = countryFileUploadValidatorService;
        _syncCountriesChannel = syncCountriesChannel;
        GlobalOptions = globalOptions;
    }

    public async Task OnGetAsync()
    {
        await LoadCountriesAsync();
    }

    public async Task<IActionResult> OnPostUploadAsync(CancellationToken cancellationToken)
    {
        if (Upload is null)
            return await HandleFileValidationAsync("File is missing");

        var uploadedFile = new UploadedFileModel
        {
            FileName = Upload.FileName,
            ContentType = Upload.ContentType
        };

        if (!_countryFileUploadValidatorService.ValidateFile(uploadedFile))
            return await HandleFileValidationAsync("Only JSON files are allowed");

        await using var stream = Upload.OpenReadStream();
        var countries = (await _countryFileUploadValidatorService.ParseFileAsync(stream)).ToArray();
        if (countries.Length == 0)
            return await HandleFileValidationAsync("Cannot parse the file or the file is empty");

        await _syncCountriesChannel.SyncAsync(countries, cancellationToken);

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            await _countryService.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (CountryServiceException exception)
        {
            SetErrorMessage(exception.Message);
            await LoadCountriesAsync();
            return Page();
        }
    }

    private async Task<PageResult> HandleFileValidationAsync(string errorMessage)
    {
        SetErrorMessage(errorMessage);
        await LoadCountriesAsync();
        return Page();
    }

    private async Task LoadCountriesAsync()
    {
        try
        {
            Countries = await _countryService.GetAllAsync();
        }
        catch (CountryServiceException exception)
        {
            SetErrorMessage(exception.Message);
            Countries = [];
        }
    }

    private void SetErrorMessage(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(ErrorMessage))
            ErrorMessage = errorMessage;
    }
}