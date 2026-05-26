namespace CountryWiki.BLL.Tests.GrpcExceptionCountryServiceDecoratorTests;

public abstract class GrpcExceptionCountryServiceDecoratorTestsBase
{
    protected readonly ICountryService Inner;
    protected readonly GrpcExceptionCountryServiceDecorator Decorator;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected GrpcExceptionCountryServiceDecoratorTestsBase()
    {
        Inner = Substitute.For<ICountryService>();
        Decorator = new GrpcExceptionCountryServiceDecorator(Inner);
    }
}