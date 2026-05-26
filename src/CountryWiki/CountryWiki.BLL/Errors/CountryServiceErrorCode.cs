namespace CountryWiki.BLL.Errors;

public enum CountryServiceErrorCode
{
    NotFound,
    ServiceUnavailable,
    Timeout,
    ValidationFailed,
    InternalError
}