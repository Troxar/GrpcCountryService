namespace CountryService.Grpc.Tests.CountryGrpcServiceTests;

public abstract class CountryGrpcServiceTestsBase
{
    protected readonly ICountryService CountryService;
    protected readonly CountryGrpcService GrpcService;
    protected readonly ServerCallContext ServerCallContext;

    protected CountryGrpcServiceTestsBase()
    {
        CountryService = Substitute.For<ICountryService>();
        GrpcService = new CountryGrpcService(CountryService);
        ServerCallContext = TestDataFactory.CreateServerCallContext();
    }
}