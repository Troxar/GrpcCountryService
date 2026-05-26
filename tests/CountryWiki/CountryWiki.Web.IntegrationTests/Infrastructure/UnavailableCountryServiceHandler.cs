namespace CountryWiki.Web.IntegrationTests.Infrastructure;

public sealed class UnavailableCountryServiceHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            RequestMessage = request,
            Version = HttpVersion.Version20,
            Content = new ByteArrayContent([])
        };

        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/grpc");
        response.TrailingHeaders.Add("grpc-status", ((int)StatusCode.Unavailable).ToString());
        response.TrailingHeaders.Add("grpc-message", "CountryService gRPC endpoint is not available.");

        return Task.FromResult(response);
    }
}