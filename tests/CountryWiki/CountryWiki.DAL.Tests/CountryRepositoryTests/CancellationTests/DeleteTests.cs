namespace CountryWiki.DAL.Tests.CountryRepositoryTests.CancellationTests;

public sealed class DeleteTests : CancellationTestsBase
{
    [Fact]
    public async Task ShouldPassCancellationTokenAndDeadline()
    {
        // Arrange
        const int countryId = 10;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var capturedDeadline = (DateTime?)null;
        var capturedCancellationToken = CancellationToken.None;

        Client.DeleteAsync(Arg.Any<CountryIdRequest>(), Arg.Any<Metadata?>(),
                Arg.Do<DateTime?>(deadline => capturedDeadline = deadline),
                Arg.Do<CancellationToken>(token => capturedCancellationToken = token))
            .Returns(CreateUnaryCall(new Empty()));

        // Act
        await Repository.DeleteAsync(countryId, cancellationToken);

        // Assert
        _ = Client.Received(1).DeleteAsync(Arg.Is<CountryIdRequest>(request => request.Id == countryId),
            Arg.Any<Metadata?>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>());

        var expectedDeadline = FixedUtcNow.AddSeconds(GrpcOptions.DefaultCallTimeoutSeconds).UtcDateTime;
        capturedDeadline.Should().Be(expectedDeadline);
        capturedCancellationToken.Should().Be(cancellationToken);
    }

    private static AsyncUnaryCall<TResponse> CreateUnaryCall<TResponse>(TResponse response)
    {
        return new AsyncUnaryCall<TResponse>(Task.FromResult(response), Task.FromResult(new Metadata()),
            () => Status.DefaultSuccess, () => [], () => { });
    }
}