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

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        await LoadCountriesAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostUploadAsync(CancellationToken cancellationToken)
    {
        if (Upload is null)
            return await HandleFileValidationAsync("File is missing", cancellationToken);

        var uploadedFile = new UploadedFileModel
        {
            FileName = Upload.FileName,
            ContentType = Upload.ContentType
        };

        if (!_countryFileUploadValidatorService.ValidateFile(uploadedFile))
            return await HandleFileValidationAsync("Only JSON files are allowed", cancellationToken);

        await using var stream = Upload.OpenReadStream();
        var countries = (await _countryFileUploadValidatorService.ParseFileAsync(stream, cancellationToken)).ToArray();
        if (countries.Length == 0)
            return await HandleFileValidationAsync("Cannot parse the file or the file is empty", cancellationToken);

        GlobalOptions.ProcessingUpload = true;
        var syncStarted = await _syncCountriesChannel.SyncAsync(countries, cancellationToken);
        if (syncStarted)
            return RedirectToPage("./Index");

        GlobalOptions.ProcessingUpload = false;
        return await HandleFileValidationAsync("Cannot start file upload processing", cancellationToken);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _countryService.DeleteAsync(id, cancellationToken);
            return RedirectToPage("./Index");
        }
        catch (CountryServiceException exception)
        {
            SetErrorMessage(exception.Message);
            await LoadCountriesAsync(cancellationToken);
            return Page();
        }
    }

    private async Task<PageResult> HandleFileValidationAsync(string errorMessage, CancellationToken cancellationToken)
    {
        SetErrorMessage(errorMessage);
        await LoadCountriesAsync(cancellationToken);
        return Page();
    }

    private async Task LoadCountriesAsync(CancellationToken cancellationToken)
    {
        if (GlobalOptions.ProcessingUpload)
        {
            Countries = [];
            return;
        }

        try
        {
            Countries = await _countryService.GetAllAsync(cancellationToken);
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