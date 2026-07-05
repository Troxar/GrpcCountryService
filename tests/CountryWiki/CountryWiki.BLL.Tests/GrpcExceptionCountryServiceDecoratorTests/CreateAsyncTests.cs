namespace CountryWiki.BLL.Tests.GrpcExceptionCountryServiceDecoratorTests;

public sealed class CreateAsyncTests : GrpcExceptionCountryServiceDecoratorTestsBase
{
    [Fact]
    public async Task ShouldThrowValidationFailed_WhenRepositoryThrowsInvalidArgument()
    {
        // Arrange
        var countriesToCreate = new[]
        {
            TestDataFactory.CreateCreateCountryModel()
        };
        var rpcException = TestDataFactory.CreateRpcException(StatusCode.InvalidArgument);
        Inner.CreateAsync(countriesToCreate, Arg.Any<CancellationToken>()).Returns(Task.FromException(rpcException));

        // Act
        var act = async () => await Decorator.CreateAsync(countriesToCreate, CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<CountryServiceException>();

        exception.Which.ErrorCode.Should().Be(CountryServiceErrorCode.ValidationFailed);
        exception.Which.Message.Should().Be(rpcException.Status.Detail);
        exception.Which.InnerException.Should().BeSameAs(rpcException);
    }

    [Fact]
    public async Task
        ShouldThrowCountryServiceException_WithAlreadyExistsCode_WhenInnerThrowsAlreadyExistsRpcException()
    {
        // Arrange
        var countriesToCreate = new[]
        {
            TestDataFactory.CreateCreateCountryModel()
        };
        var detail = Guid.NewGuid().ToString();
        var rpcException = TestDataFactory.CreateRpcException(StatusCode.AlreadyExists, detail);
        Inner.CreateAsync(countriesToCreate, CancellationToken).Returns(Task.FromException(rpcException));

        // Act
        var act = async () => await Decorator.CreateAsync(countriesToCreate, CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<CountryServiceException>();

        exception.Which.ErrorCode.Should().Be(CountryServiceErrorCode.AlreadyExists);
        exception.Which.Message.Should().Be(detail);
        exception.Which.InnerException.Should().BeSameAs(rpcException);

        await Inner.Received(1).CreateAsync(countriesToCreate, CancellationToken);
    }
}