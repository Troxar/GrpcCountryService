using CountryServiceClient = CountryService.Grpc.v1.CountryService.CountryServiceClient;

namespace CountryWiki.DAL.Tests.CountryRepositoryTests.CancellationTests;

public abstract class CancellationTestsBase
{
    protected readonly CountryServiceClient Client;
    protected readonly CountryRepository Repository;
    protected readonly DateTimeOffset FixedUtcNow;
    protected readonly GrpcOptions GrpcOptions;

    protected CancellationTestsBase()
    {
        Client = Substitute.For<CountryServiceClient>();
        GrpcOptions = new GrpcOptions
        {
            DefaultCallTimeoutSeconds = 5
        };
        FixedUtcNow = new DateTimeOffset(DateTime.UtcNow);

        var options = Options.Create(GrpcOptions);
        var timeProvider = new FakeTimeProvider(FixedUtcNow);
        Repository = new CountryRepository(Client, options, timeProvider);
    }
}