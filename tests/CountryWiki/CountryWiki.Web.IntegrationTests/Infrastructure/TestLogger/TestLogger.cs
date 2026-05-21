namespace CountryWiki.Web.IntegrationTests.Infrastructure.TestLogger;

public sealed class TestLogger : ILogger
{
    private readonly string _categoryName;
    private readonly List<TestLogRecord> _records;

    public TestLogger(string categoryName, List<TestLogRecord> records)
    {
        _categoryName = categoryName;
        _records = records;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return NullScope.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        lock (_records)
        {
            _records.Add(new TestLogRecord(_categoryName, logLevel, eventId, message, exception));
        }
    }
}