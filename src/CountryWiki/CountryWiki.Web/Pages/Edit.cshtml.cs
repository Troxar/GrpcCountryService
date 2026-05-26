namespace CountryWiki.Web.Pages;

public class EditModel : PageModel
{
    private readonly ICountryService _countryService;

    public string CountryName { get; set; } = string.Empty;

    [BindProperty]
    public UpdateCountry CountryToUpdate { get; set; } = new();

    public EditModel(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        return await LoadCountryPageAsync(id, true, cancellationToken);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return await LoadCountryPageAsync(CountryToUpdate.Id, false, cancellationToken);

        try
        {
            var model = new UpdateCountryModel
            {
                Id = CountryToUpdate.Id,
                Description = CountryToUpdate.Description
            };
            await _countryService.UpdateAsync(model, cancellationToken);

            return RedirectToPage("./Index");
        }
        catch (CountryServiceException exception)
        {
            return this.ToActionResult(exception);
        }
    }

    private async Task<IActionResult> LoadCountryPageAsync(int id, bool updateCountryToUpdate,
        CancellationToken cancellationToken)
    {
        try
        {
            var country = await _countryService.GetAsync(id, cancellationToken);
            if (country is null)
                return NotFound();

            CountryName = country.Name;
            if (updateCountryToUpdate)
                CountryToUpdate = new UpdateCountry
                {
                    Id = country.Id,
                    Description = country.Description
                };

            return Page();
        }
        catch (CountryServiceException exception)
        {
            return this.ToActionResult(exception);
        }
    }
}