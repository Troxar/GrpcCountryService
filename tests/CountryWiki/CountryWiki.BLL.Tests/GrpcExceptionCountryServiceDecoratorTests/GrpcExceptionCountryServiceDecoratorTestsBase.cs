namespace CountryWiki.BLL.Tests.GrpcExceptionCountryServiceDecoratorTests;

public abstract class GrpcExceptionCountryServiceDecoratorTestsBase
{
    protected readonly ICountryService Inner;
    protected readonly GrpcExceptionCountryServiceDecorator Decorator;

    protected GrpcExceptionCountryServiceDecoratorTestsBase()
    {
        Inner = Substitute.For<ICountryService>();
        Decorator = new GrpcExceptionCountryServiceDecorator(Inner);
    }
}