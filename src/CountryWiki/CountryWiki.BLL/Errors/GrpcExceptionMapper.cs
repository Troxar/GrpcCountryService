namespace CountryWiki.BLL.Errors;

public static class GrpcExceptionMapper
{
    extension(RpcException exception)
    {
        public CountryServiceException ToCountryServiceException()
        {
            return exception.StatusCode switch
            {
                StatusCode.NotFound => new CountryServiceException(CountryServiceErrorCode.NotFound,
                    "The requested country was not found", exception),
                StatusCode.Unavailable => new CountryServiceException(CountryServiceErrorCode.ServiceUnavailable,
                    "Country service is temporarily unavailable", exception),
                StatusCode.DeadlineExceeded => new CountryServiceException(CountryServiceErrorCode.Timeout,
                    "Country service did not respond within the time limit", exception),
                StatusCode.InvalidArgument => new CountryServiceException(CountryServiceErrorCode.ValidationFailed,
                    exception.Status.Detail is { Length: > 0 } ? exception.Status.Detail : "Country request is invalid",
                    exception),
                _ => new CountryServiceException(CountryServiceErrorCode.InternalError,
                    "Country service request failed", exception)
            };
        }
    }
}