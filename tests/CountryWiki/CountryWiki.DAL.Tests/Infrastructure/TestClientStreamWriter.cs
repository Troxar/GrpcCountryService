namespace CountryWiki.DAL.Tests.Infrastructure;

internal class TestClientStreamWriter<T> : IClientStreamWriter<T>
{
    public List<T> Messages { get; } = [];
    public WriteOptions? WriteOptions { get; set; }
    public bool IsCompleted { get; private set; }

    public Task WriteAsync(T message)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Cannot write after stream has been completed");

        Messages.Add(message);
        return Task.CompletedTask;
    }

    public Task CompleteAsync()
    {
        IsCompleted = true;
        return Task.CompletedTask;
    }
}