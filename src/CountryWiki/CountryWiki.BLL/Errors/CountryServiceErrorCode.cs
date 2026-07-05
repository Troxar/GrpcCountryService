namespace CountryWiki.BLL.Errors;

public enum CountryServiceErrorCode
{
    NotFound,
    AlreadyExists,
    ServiceUnavailable,
    Timeout,
    ValidationFailed,
    InternalError
}