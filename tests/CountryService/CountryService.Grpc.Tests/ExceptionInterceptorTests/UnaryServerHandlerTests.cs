namespace CountryService.Grpc.Tests.ExceptionInterceptorTests;

public class UnaryServerHandlerTests : ExceptionInterceptorTestsBase
{
    private const string CorrelationId = "test-correlation-id";
    private readonly ServerCallContext _context = TestDataFactory.CreateServerCallContext(CorrelationId);
    private readonly Empty _request = new();

    [Fact]
    public async Task ShouldReturnResponse_WhenContinuationSucceeds()
    {
        // Arrange
        var expectedResponse = new Empty();

        // Act
        var response = await Interceptor.UnaryServerHandler(_request, _context,
            (_, _) => Task.FromResult(expectedResponse));

        // Assert
        response.Should().BeSameAs(expectedResponse);
    }

    [Fact]
    public async Task ShouldPreserveStatus_AndAddCorrelationIdTrailer_WhenContinuationThrowsRpcException()
    {
        // Arrange
        var status = new Status(StatusCode.NotFound, Guid.NewGuid().ToString());
        var sourceException = new RpcException(status);

        // Act
        var act = async () => await Interceptor.UnaryServerHandler<Empty, Empty>(_request, _context,
            (_, _) => throw sourceException);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(status.StatusCode);
        exception.Which.Status.Detail.Should().Be(status.Detail);
        exception.Which.Trailers.GetValue("x-correlation-id").Should().Be(CorrelationId);
    }

    [Fact]
    public async Task ShouldReturnInternal_AndAddCorrelationIdTrailer_WhenContinuationThrowsTimeoutException()
    {
        // Act
        var act = async () => await Interceptor.UnaryServerHandler<Empty, Empty>(_request, _context,
            (_, _) => throw new TimeoutException());

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.Internal);
        exception.Which.Status.Detail.Should().Be("An external resource did not answer within the time limit");
        exception.Which.Trailers.GetValue(MetadataNames.CorrelationId).Should().Be(CorrelationId);
    }

    [Fact]
    public async Task ShouldReturnInternal_AndAddCorrelationIdTrailer_WhenContinuationThrowsDefaultException()
    {
        // Arrange
        var sourceException = new InvalidOperationException(Guid.NewGuid().ToString());

        // Act
        var act = async () => await Interceptor.UnaryServerHandler<Empty, Empty>(_request, _context,
            (_, _) => throw sourceException);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.Internal);
        exception.Which.Status.Detail.Should().Be("Internal server error");
        exception.Which.Trailers.GetValue(MetadataNames.CorrelationId).Should().Be(CorrelationId);
    }

    [Fact]
    public async Task ShouldUseDefaultCorrelationId_WhenCorrelationIdIsMissing()
    {
        // Arrange
        var context = TestDataFactory.CreateServerCallContext();
        var sourceException = new InvalidOperationException(Guid.NewGuid().ToString());

        // Act
        var act = async () => await Interceptor.UnaryServerHandler<Empty, Empty>(_request, context,
            (_, _) => throw sourceException);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.Trailers.GetValue(MetadataNames.CorrelationId).Should().Be($"no-{MetadataNames.CorrelationId}");
    }
}