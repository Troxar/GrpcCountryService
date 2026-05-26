namespace CountryWiki.DAL.Tests.Infrastructure;

internal sealed class CancellationAwareAsyncStreamReader<T> : IAsyncStreamReader<T>
{
    public T Current => default!;

    public CancellationToken CapturedCancellationToken { get; private set; }

    public Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        CapturedCancellationToken = cancellationToken;
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(false);
    }
}