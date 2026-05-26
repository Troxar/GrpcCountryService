namespace CountryWiki.BLL.Tests.GrpcExceptionCountryServiceDecoratorTests;

public sealed class UpdateAsyncTests : GrpcExceptionCountryServiceDecoratorTestsBase
{
    [Fact]
    public async Task ShouldThrowCountryServiceException_WithTimeout_WhenRepositoryThrowsDeadlineExceeded()
    {
        // Arrange
        var countryToUpdate = TestDataFactory.CreateUpdateCountryModel(10);
        var rpcException = TestDataFactory.CreateRpcException(StatusCode.DeadlineExceeded);
        Inner.UpdateAsync(countryToUpdate, Arg.Any<CancellationToken>()).Returns(Task.FromException(rpcException));

        // Act
        var act = async () => await Decorator.UpdateAsync(countryToUpdate, CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<CountryServiceException>();

        exception.Which.ErrorCode.Should().Be(CountryServiceErrorCode.Timeout);
        exception.Which.Message.Should().Be("Country service did not respond within the time limit");
        exception.Which.InnerException.Should().BeSameAs(rpcException);
    }
}