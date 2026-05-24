namespace CountryService.Grpc.IntegrationTests.HealthChecksTests;

public abstract class HealthChecksTestsBase : IClassFixture<CountryGrpcIntegrationFixture>
{
    protected readonly CountryGrpcIntegrationFixture Fixture;
    protected readonly HttpClient HttpClient;
    protected readonly GrpcChannel GrpcChannel;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected HealthChecksTestsBase(CountryGrpcIntegrationFixture fixture)
    {
        Fixture = fixture;
        HttpClient = fixture.Factory.CreateClient();
        GrpcChannel = fixture.Factory.CreateGrpcChannel();
    }
}