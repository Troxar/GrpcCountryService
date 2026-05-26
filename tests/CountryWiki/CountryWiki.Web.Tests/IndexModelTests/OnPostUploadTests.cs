namespace CountryWiki.Web.Tests.IndexModelTests;

public sealed class OnPostUploadTests : IndexModelTestsBase
{
    [Fact]
    public async Task ShouldReturnPageWithError_AndLoadCountries_WhenFileIsMissing()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCountryModel(1)
        };
        CountryService.GetAllAsync().Returns(countries);
        IndexModel.Upload = null;

        // Act
        var result = await IndexModel.OnPostUploadAsync(CancellationToken.None);

        // Assert
        result.Should().BeOfType<PageResult>();

        IndexModel.ErrorMessage.Should().Be("File is missing");
        IndexModel.Countries.Should().BeEquivalentTo(countries);
        FileUploadValidatorService.DidNotReceive().ValidateFile(Arg.Any<UploadedFileModel>());

        await CountryService.Received(1).GetAllAsync();
        await FileUploadValidatorService.DidNotReceive().ParseFileAsync(Arg.Any<Stream>());
        await SyncCountriesChannel.DidNotReceive()
            .SyncAsync(Arg.Any<IEnumerable<CreateCountryModel>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldReturnPageWithError_AndLoadCountries_WhenFileIsInvalid()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCountryModel(1)
        };
        CountryService.GetAllAsync().Returns(countries);
        IndexModel.Upload = TestDataFactory.CreateFormFile("countries.txt", MediaTypeNames.Text.Plain, "not json");
        FileUploadValidatorService.ValidateFile(Arg.Is<UploadedFileModel>(x =>
                x.FileName == "countries.txt"
                && x.ContentType == MediaTypeNames.Text.Plain))
            .Returns(false);

        // Act
        var result = await IndexModel.OnPostUploadAsync(CancellationToken);

        // Assert
        result.Should().BeOfType<PageResult>();

        IndexModel.ErrorMessage.Should().Be("Only JSON files are allowed");
        IndexModel.Countries.Should().BeEquivalentTo(countries);
        FileUploadValidatorService.Received(1).ValidateFile(Arg.Is<UploadedFileModel>(x =>
            x.FileName == "countries.txt"
            && x.ContentType == MediaTypeNames.Text.Plain));

        await FileUploadValidatorService.DidNotReceive().ParseFileAsync(Arg.Any<Stream>());
        await SyncCountriesChannel.DidNotReceive()
            .SyncAsync(Arg.Any<IEnumerable<CreateCountryModel>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldReturnPageWithError_AndLoadCountries_WhenFileCannotBeParsed()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCountryModel(1)
        };
        CountryService.GetAllAsync().Returns(countries);
        IndexModel.Upload = TestDataFactory.CreateFormFile("countries.json", MediaTypeNames.Application.Json,
            "invalid json");
        FileUploadValidatorService.ValidateFile(Arg.Any<UploadedFileModel>()).Returns(true);
        FileUploadValidatorService.ParseFileAsync(Arg.Any<Stream>()).Returns([]);

        // Act
        var result = await IndexModel.OnPostUploadAsync(CancellationToken);

        // Assert
        result.Should().BeOfType<PageResult>();

        IndexModel.ErrorMessage.Should().Be("Cannot parse the file or the file is empty");
        IndexModel.Countries.Should().BeEquivalentTo(countries);
        FileUploadValidatorService.Received(1).ValidateFile(Arg.Is<UploadedFileModel>(x =>
            x.FileName == "countries.json"
            && x.ContentType == MediaTypeNames.Application.Json));

        await FileUploadValidatorService.Received(1).ParseFileAsync(Arg.Any<Stream>());
        await SyncCountriesChannel.DidNotReceive()
            .SyncAsync(Arg.Any<IEnumerable<CreateCountryModel>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldSyncCountries_AndRedirectToIndex_WhenFileIsValidAndParsed()
    {
        // Arrange
        var countries = new[]
        {
            TestDataFactory.CreateCreateCountryModel(),
            TestDataFactory.CreateCreateCountryModel()
        };
        const string content = """
                               [
                                 { "name": "France" },
                                 { "name": "Germany" }
                               ]
                               """;
        IndexModel.Upload = TestDataFactory.CreateFormFile("countries.json", MediaTypeNames.Application.Json, content);
        FileUploadValidatorService.ValidateFile(Arg.Any<UploadedFileModel>()).Returns(true);
        FileUploadValidatorService.ParseFileAsync(Arg.Any<Stream>()).Returns(countries);
        SyncCountriesChannel.SyncAsync(Arg.Any<IEnumerable<CreateCountryModel>>(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await IndexModel.OnPostUploadAsync(CancellationToken);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToPageResult>().Subject;
        redirectResult.PageName.Should().Be("./Index");

        IndexModel.ErrorMessage.Should().BeEmpty();

        await SyncCountriesChannel.Received(1)
            .SyncAsync(Arg.Is<IEnumerable<CreateCountryModel>>(x => x.SequenceEqual(countries)), CancellationToken);
        await CountryService.DidNotReceive().GetAllAsync();
    }

    [Fact]
    public async Task ShouldKeepUploadError_WhenReloadingCountriesFails()
    {
        // Arrange
        CountryService.GetAllAsync().Returns<IEnumerable<CountryModel>>(_ =>
            throw new CountryServiceException(CountryServiceErrorCode.ServiceUnavailable, Guid.NewGuid().ToString()));
        IndexModel.Upload = null;

        // Act
        var result = await IndexModel.OnPostUploadAsync(CancellationToken.None);

        // Assert
        result.Should().BeOfType<PageResult>();

        IndexModel.ErrorMessage.Should().Be("File is missing");
        IndexModel.Countries.Should().BeEmpty();
        FileUploadValidatorService.DidNotReceive().ValidateFile(Arg.Any<UploadedFileModel>());

        await CountryService.Received(1).GetAllAsync();
        await FileUploadValidatorService.DidNotReceive().ParseFileAsync(Arg.Any<Stream>());
        await SyncCountriesChannel.DidNotReceive()
            .SyncAsync(Arg.Any<IEnumerable<CreateCountryModel>>(), Arg.Any<CancellationToken>());
    }
}