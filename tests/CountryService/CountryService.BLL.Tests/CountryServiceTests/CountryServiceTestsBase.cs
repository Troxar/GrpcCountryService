namespace CountryService.BLL.Tests.CountryServiceTests;

public abstract class CountryServiceTestsBase
{
    protected readonly ICountryRepository CountryRepository;
    protected readonly Services.CountryService CountryService;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected CountryServiceTestsBase()
    {
        CountryRepository = Substitute.For<ICountryRepository>();
        CountryService = new Services.CountryService(CountryRepository);
    }
}