namespace CountryWiki.Web.IntegrationTests.Infrastructure.TestLogger;

internal class NullScope : IDisposable
{
    public static NullScope Instance { get; } = new();

    public void Dispose()
    {
    }

    private NullScope()
    {
    }
}