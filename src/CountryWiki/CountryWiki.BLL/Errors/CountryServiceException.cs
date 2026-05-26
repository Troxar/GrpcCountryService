namespace CountryWiki.BLL.Errors;

public sealed class CountryServiceException : Exception
{
    public CountryServiceErrorCode ErrorCode { get; }

    public CountryServiceException(CountryServiceErrorCode errorCode, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}