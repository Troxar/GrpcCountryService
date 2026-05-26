namespace CountryWiki.Web.Tests.IndexModelTests;

public abstract class IndexModelTestsBase
{
    protected readonly ICountryService CountryService;
    protected readonly IFileUploadValidatorService FileUploadValidatorService;
    protected readonly ISyncCountriesChannel SyncCountriesChannel;
    protected readonly GlobalOptions GlobalOptions;
    protected readonly IndexModel IndexModel;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected IndexModelTestsBase()
    {
        CountryService = Substitute.For<ICountryService>();
        FileUploadValidatorService = Substitute.For<IFileUploadValidatorService>();
        SyncCountriesChannel = Substitute.For<ISyncCountriesChannel>();
        GlobalOptions = new GlobalOptions();
        IndexModel = new IndexModel(CountryService, FileUploadValidatorService, SyncCountriesChannel, GlobalOptions);
    }
}