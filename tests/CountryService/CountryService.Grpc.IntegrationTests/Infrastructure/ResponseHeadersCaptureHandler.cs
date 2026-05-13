namespace CountryService.Grpc.IntegrationTests.Infrastructure;

public class ResponseHeadersCaptureHandler : DelegatingHandler
{
    public ResponseHeadersCaptureHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
    }

    public HttpResponseHeaders? LastResponseHeaders { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        LastResponseHeaders = response.Headers;
        return response;
    }
}