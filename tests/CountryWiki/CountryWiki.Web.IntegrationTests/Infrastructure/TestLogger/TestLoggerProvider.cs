namespace CountryWiki.Web.IntegrationTests.Infrastructure.TestLogger;

public class TestLoggerProvider : ILoggerProvider
{
    private readonly List<TestLogRecord> _records = [];

    public IReadOnlyList<TestLogRecord> Records
    {
        get
        {
            lock (_records)
            {
                return _records;
            }
        }
    }

    public void Clear()
    {
        lock (_records)
        {
            _records.Clear();
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(categoryName, _records);
    }

    public void Dispose()
    {
    }
}