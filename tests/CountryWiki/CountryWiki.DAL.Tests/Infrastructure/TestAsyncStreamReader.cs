namespace CountryWiki.DAL.Tests.Infrastructure;

internal class TestAsyncStreamReader<T> : IAsyncStreamReader<T>
{
    private readonly IEnumerator<T> _enumerator;

    internal TestAsyncStreamReader(IEnumerable<T> items)
    {
        _enumerator = items.GetEnumerator();
    }

    public Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        return Task.FromResult(_enumerator.MoveNext());
    }

    public T Current => _enumerator.Current;
}