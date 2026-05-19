namespace CountryWiki.Web.Interceptors;

public partial class TracerInterceptor : Interceptor
{
    private readonly ILogger<TracerInterceptor> _logger;

    public TracerInterceptor(ILogger<TracerInterceptor> logger)
    {
        _logger = logger;
    }

    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        LogMethodExecutingOnServerContextHost(context);
        return continuation(context);
    }

    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        LogMethodExecutingOnService(context);
        return continuation(context);
    }

    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        LogMethodExecutingOnService(context);
        return continuation(request, context);
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        LogMethodExecutingOnService(context);
        return continuation(request, context);
    }

    private void LogMethodExecutingOnServerContextHost<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context)
        where TRequest : class
        where TResponse : class
    {
        if (context.Host is null)
            return;

        LogMethodExecutingOnServerContextHost(context.Method.Name, context.Method.Type, context.Method.ServiceName);
    }

    private void LogMethodExecutingOnService<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context)
        where TRequest : class
        where TResponse : class
    {
        LogMethodExecutingOnService(context.Method.Name, context.Method.Type, context.Method.ServiceName);
    }

    [LoggerMessage(LogLevel.Debug, "Executing {MethodName} {MethodType} method on server {ContextHost}")]
    partial void LogMethodExecutingOnServerContextHost(string methodName, MethodType methodType, string contextHost);

    [LoggerMessage(LogLevel.Debug, "Executing {MethodName} {MethodType} method on service {ContextHost}")]
    partial void LogMethodExecutingOnService(string methodName, MethodType methodType, string contextHost);
}