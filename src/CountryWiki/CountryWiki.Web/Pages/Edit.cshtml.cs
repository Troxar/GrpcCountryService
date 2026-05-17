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

    public async Task OnGetAsync(int id)
    {
        await RetrieveCountryAsync(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await RetrieveCountryAsync(CountryToUpdate.Id);
            return Page();
        }

        var model = new UpdateCountryModel
        {
            Id = CountryToUpdate.Id,
            Description = CountryToUpdate.Description
        };
        await _countryService.UpdateAsync(model);

        return RedirectToPage("./Index");
    }

    private async Task RetrieveCountryAsync(int id)
    {
        var country = await _countryService.GetAsync(id);
        if (country is null)
            return;

        CountryName = country.Name;
        CountryToUpdate = new UpdateCountry
        {
            Id = country.Id,
            Description = country.Description
        };
    }
}