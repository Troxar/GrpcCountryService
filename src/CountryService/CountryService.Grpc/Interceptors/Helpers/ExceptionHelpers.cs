namespace CountryService.Grpc.Interceptors.Helpers;

public static class ExceptionHelpers
{
    public static RpcException Handle<T>(this Exception exception, ServerCallContext context, ILogger<T> logger)
    {
        var correlationId = context.RequestHeaders.Get(MetadataNames.CorrelationId)?.Value
                            ?? $"no-{MetadataNames.CorrelationId}";
        return exception switch
        {
            TimeoutException timeoutException => HandleTimeoutException(timeoutException, correlationId, logger),
            RpcException rpcException => HandleRpcException(rpcException, correlationId, logger),
            _ => HandleDefaultException(exception, correlationId, logger)
        };
    }

    private static RpcException HandleTimeoutException<T>(TimeoutException exception,
        string correlationId, ILogger<T> logger)
    {
        logger.LogError(exception, "A timeout occured. CorrelationId: {CorrelationId}", correlationId);
        var status = new Status(StatusCode.Internal, "An external resource did not answer within the time limit");
        return new RpcException(status, CreateTrailers(correlationId));
    }

    private static RpcException HandleRpcException<T>(RpcException exception,
        string correlationId, ILogger<T> logger)
    {
        logger.LogError(exception, "An error occured. CorrelationId: {CorrelationId}", correlationId);
        return new RpcException(exception.Status, CreateTrailers(correlationId));
    }

    private static RpcException HandleDefaultException<T>(Exception exception,
        string correlationId, ILogger<T> logger)
    {
        logger.LogError(exception, "An error occured. CorrelationId: {CorrelationId}", correlationId);
        var status = new Status(StatusCode.Internal, "Internal server error");
        return new RpcException(status, CreateTrailers(correlationId));
    }

    private static Metadata CreateTrailers(string correlationId)
    {
        var trailers = new Metadata
        {
            { MetadataNames.CorrelationId, correlationId }
        };
        return trailers;
    }
}