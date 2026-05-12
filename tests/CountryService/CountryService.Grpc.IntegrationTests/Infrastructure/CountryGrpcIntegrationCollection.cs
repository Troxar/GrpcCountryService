namespace CountryService.Grpc.IntegrationTests.Infrastructure;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class CountryGrpcIntegrationCollection : ICollectionFixture<CountryGrpcIntegrationFixture>
{
    public const string Name = "Country gRPC integration collection";
}