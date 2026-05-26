namespace CountryWiki.BLL.Tests.GrpcExceptionCountryServiceDecoratorTests;

public sealed class DeleteAsyncTests : GrpcExceptionCountryServiceDecoratorTestsBase
{
    [Fact]
    public async Task ShouldThrowCountryServiceException_WithInternalError_WhenRepositoryThrowsInternal()
    {
        // Arrange
        const int countryId = 10;
        var rpcException = TestDataFactory.CreateRpcException(StatusCode.Internal);
        Inner.DeleteAsync(countryId).Returns(Task.FromException(rpcException));

        // Act
        var act = async () => await Decorator.DeleteAsync(countryId);

        // Assert
        var exception = await act.Should().ThrowAsync<CountryServiceException>();

        exception.Which.ErrorCode.Should().Be(CountryServiceErrorCode.InternalError);
        exception.Which.Message.Should().Be("Country service request failed");
        exception.Which.InnerException.Should().BeSameAs(rpcException);
    }
}