namespace CountryService.Grpc.Tests.ExceptionInterceptorTests;

public abstract class ExceptionInterceptorTestsBase
{
    protected readonly ExceptionInterceptor Interceptor;

    protected ExceptionInterceptorTestsBase()
    {
        var logger = Substitute.For<ILogger<ExceptionInterceptor>>();
        Interceptor = new ExceptionInterceptor(logger);
    }
}