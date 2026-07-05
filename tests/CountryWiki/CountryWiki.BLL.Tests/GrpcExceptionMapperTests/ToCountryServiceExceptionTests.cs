namespace CountryWiki.BLL.Tests.GrpcExceptionMapperTests;

public sealed class ToCountryServiceExceptionTests
{
    [Theory]
    [MemberData(nameof(StatusCodeMappings))]
    public void ShouldMapStatusCodeToCountryServiceErrorCode(StatusCode statusCode,
        CountryServiceErrorCode expectedErrorCode)
    {
        // Arrange
        var rpcException = TestDataFactory.CreateRpcException(statusCode);

        // Act
        var exception = rpcException.ToCountryServiceException();

        // Assert
        exception.ErrorCode.Should().Be(expectedErrorCode);
        exception.InnerException.Should().BeSameAs(rpcException);
    }

    public static TheoryData<StatusCode, CountryServiceErrorCode> StatusCodeMappings => new()
    {
        { StatusCode.NotFound, CountryServiceErrorCode.NotFound },
        { StatusCode.AlreadyExists, CountryServiceErrorCode.AlreadyExists },
        { StatusCode.Unavailable, CountryServiceErrorCode.ServiceUnavailable },
        { StatusCode.DeadlineExceeded, CountryServiceErrorCode.Timeout },
        { StatusCode.InvalidArgument, CountryServiceErrorCode.ValidationFailed },
        { StatusCode.Unknown, CountryServiceErrorCode.InternalError }
    };
}