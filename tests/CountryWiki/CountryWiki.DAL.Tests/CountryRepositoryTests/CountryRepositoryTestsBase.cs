using CountryServiceClient = CountryService.Grpc.v1.CountryService.CountryServiceClient;

namespace CountryWiki.DAL.Tests.CountryRepositoryTests;

public abstract class CountryRepositoryTestsBase
{
    protected readonly CountryServiceClient Client;
    protected readonly CountryRepository Repository;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected CountryRepositoryTestsBase()
    {
        Client = Substitute.For<CountryServiceClient>();
        Repository = new CountryRepository(Client, Options.Create(new GrpcOptions()));
    }
}