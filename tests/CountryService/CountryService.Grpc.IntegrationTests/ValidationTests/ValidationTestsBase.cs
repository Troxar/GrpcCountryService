namespace CountryService.Grpc.IntegrationTests.ValidationTests;

[Collection(CountryGrpcIntegrationCollection.Name)]
public abstract class ValidationTestsBase
{
    protected readonly CountryGrpcIntegrationFixture Fixture;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected ValidationTestsBase(CountryGrpcIntegrationFixture fixture)
    {
        Fixture = fixture;
    }

    protected CountryContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<CountryContext>()
            .UseNpgsql(Fixture.Postgres.GetConnectionString())
            .Options;

        return new CountryContext(options);
    }
}