namespace CountryService.Grpc.Tests.Extensions;

public static class ValidationResultExtensions
{
    extension(ValidationResult result)
    {
        public void ShouldHaveErrorFor(string propertyName)
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x =>
                x.PropertyName == propertyName
                || x.PropertyName.StartsWith($"{propertyName}["));
        }
    }
}