namespace CountryService.Grpc.IntegrationTests.ValidationTests;

public sealed class CreateTests(CountryGrpcIntegrationFixture fixture) : ValidationTestsBase(fixture)
{
    [Fact]
    public async Task ShouldReturnInvalidArgument_WhenNameIsEmpty()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.Name = string.Empty;

        // Act
        var act = CreateAct(request);

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.InvalidArgument);
        exception.Which.Status.Detail.Should().Contain("Name");
    }

    [Fact]
    public async Task ShouldReturnInvalidArgument_WhenLanguagesContainDuplicates()
    {
        // Arrange
        var request = TestDataFactory.CreateCountryCreateRequest();
        request.Languages.AddRange([1, 1]);

        // Act
        var act = async () =>
        {
            using var call = Fixture.Client.Create(cancellationToken: CancellationToken);
            await call.RequestStream.WriteAsync(request, CancellationToken);
            await call.RequestStream.CompleteAsync();

            while (await call.ResponseStream.MoveNext(CancellationToken)) _ = call.ResponseStream.Current;
        };

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.InvalidArgument);
        exception.Which.Status.Detail.Should().Contain("Languages");
    }

    private Func<Task> CreateAct(CountryCreateRequest request)
    {
        return async () =>
        {
            using var call = Fixture.Client.Create(cancellationToken: CancellationToken);
            await call.RequestStream.WriteAsync(request, CancellationToken);
            await call.RequestStream.CompleteAsync();

            while (await call.ResponseStream.MoveNext(CancellationToken)) _ = call.ResponseStream.Current;
        };
    }
}