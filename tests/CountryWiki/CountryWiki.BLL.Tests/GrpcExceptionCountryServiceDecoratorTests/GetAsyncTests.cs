namespace CountryWiki.BLL.Tests.GrpcExceptionCountryServiceDecoratorTests;

public sealed class GetAsyncTests : GrpcExceptionCountryServiceDecoratorTestsBase
{
    [Fact]
    public async Task ShouldReturnNull_WhenRepositoryThrowsNotFoundRpcException()
    {
        // Arrange
        const int countryId = 10;
        var rpcException = TestDataFactory.CreateRpcException(StatusCode.NotFound);
        Inner.GetAsync(countryId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<CountryModel?>(rpcException));

        // Act
        var result = await Decorator.GetAsync(countryId, CancellationToken);

        // Assert
        result.Should().BeNull();

        await Inner.Received(1).GetAsync(countryId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldThrowCountryServiceException_WithInternalError_WhenRepositoryThrowsInternal()
    {
        // Arrange
        const int countryId = 10;
        var rpcException = TestDataFactory.CreateRpcException(StatusCode.Internal);
        Inner.GetAsync(countryId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<CountryModel?>(rpcException));

        // Act
        var act = async () => await Decorator.GetAsync(countryId, CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<CountryServiceException>();

        exception.Which.ErrorCode.Should().Be(CountryServiceErrorCode.InternalError);
        exception.Which.Message.Should().Be("Country service request failed");
        exception.Which.InnerException.Should().BeSameAs(rpcException);
    }

    [Fact]
    public async Task ShouldReturnCountry_WhenInnerServiceSucceeds()
    {
        // Arrange
        var country = TestDataFactory.CreateCountryModel(1);
        Inner.GetAsync(country.Id, Arg.Any<CancellationToken>()).Returns(Task.FromResult<CountryModel?>(country));

        // Act
        var result = await Decorator.GetAsync(country.Id, CancellationToken);

        // Assert
        result.Should().BeEquivalentTo(result);
    }
}