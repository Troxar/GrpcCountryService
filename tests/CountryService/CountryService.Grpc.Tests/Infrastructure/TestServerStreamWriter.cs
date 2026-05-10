namespace CountryService.Grpc.Tests.Infrastructure;

internal class TestServerStreamWriter<T> : IServerStreamWriter<T>
{
    public List<T> Messages { get; } = [];
    
    public Task WriteAsync(T message)
    {
        Messages.Add(message);
        return Task.CompletedTask;
    }

    public WriteOptions? WriteOptions { get; set; }
}