namespace CountryWiki.Web.IntegrationTests.Infrastructure;

public sealed class UnavailableCountryServiceHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        throw new HttpRequestException("CountryService gRPC endpoint is not available.");
    }
}