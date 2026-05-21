namespace CountryWiki.Web.IntegrationTests.Infrastructure.TestLogger;

public sealed record TestLogRecord(
    string CategoryName,
    LogLevel Level,
    EventId EventId,
    string Message,
    Exception? Exception);