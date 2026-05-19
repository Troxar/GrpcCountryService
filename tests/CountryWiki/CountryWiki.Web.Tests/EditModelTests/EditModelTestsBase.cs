namespace CountryWiki.Web.Tests.EditModelTests;

public abstract class EditModelTestsBase
{
    protected readonly ICountryService CountryService;
    protected readonly EditModel EditModel;

    protected EditModelTestsBase()
    {
        CountryService = Substitute.For<ICountryService>();
        EditModel = new EditModel(CountryService);
    }
}