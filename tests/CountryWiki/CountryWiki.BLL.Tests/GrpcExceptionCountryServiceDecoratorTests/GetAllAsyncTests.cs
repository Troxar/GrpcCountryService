namespace CountryWiki.BLL.Tests.GrpcExceptionCountryServiceDecoratorTests;

public sealed class GetAllAsyncTests : GrpcExceptionCountryServiceDecoratorTestsBase
{
    [Fact]
    public async Task ShouldThrowCountryServiceException_WithServiceUnavailable_WhenRepositoryThrowsUnavailable()
    {
        // Arrange
        var rpcException = TestDataFactory.CreateRpcException(StatusCode.Unavailable);
        Inner.GetAllAsync().Returns(Task.FromException<IEnumerable<CountryModel>>(rpcException));

        // Act
        var act = async () => await Decorator.GetAllAsync();

        // Assert
        var exception = await act.Should().ThrowAsync<CountryServiceException>();

        exception.Which.ErrorCode.Should().Be(CountryServiceErrorCode.ServiceUnavailable);
        exception.Which.Message.Should().Be("Country service is temporarily unavailable");
        exception.Which.InnerException.Should().BeSameAs(rpcException);
    }

    [Fact]
    public async Task ShouldReturnCountries_WhenInnerServiceSucceeds()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCountryModel(1)
        };
        Inner.GetAllAsync().Returns(Task.FromResult<IEnumerable<CountryModel>>(countries));

        // Act
        var result = await Decorator.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(countries);
    }
}