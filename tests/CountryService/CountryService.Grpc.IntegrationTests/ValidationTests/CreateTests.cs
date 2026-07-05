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

    [Fact]
    public async Task ShouldReturnAlreadyExists_WhenCountryNameAlreadyExists()
    {
        // Arrange
        await using var context = CreateContext();

        var country = TestDataFactory.CreateCountry();

        await context.Countries.AddAsync(country, CancellationToken);
        await context.SaveChangesAsync(CancellationToken);

        var request = TestDataFactory.CreateCountryCreateRequest(country.Name);

        // Act
        var act = async () =>
        {
            using var call = Fixture.Client.Create(cancellationToken: CancellationToken);
            await call.RequestStream.WriteAsync(request, CancellationToken);
            await call.RequestStream.CompleteAsync();

            while (await call.ResponseStream.MoveNext(CancellationToken))
                _ = call.ResponseStream.Current;
        };

        // Assert
        var exception = await act.Should().ThrowAsync<RpcException>();
        exception.Which.StatusCode.Should().Be(StatusCode.AlreadyExists);
        exception.Which.Status.Detail.Should().Contain(country.Name);

        await using var assertContext = CreateContext();

        var countriesCount = await assertContext.Countries.CountAsync(x => x.Name == country.Name, CancellationToken);
        countriesCount.Should().Be(1);
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