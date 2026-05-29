namespace CountryService.Grpc.Extensions;

public static class ValidationResultExtensions
{
    extension(ValidationResult validationResult)
    {
        public void ThrowIfInvalid()
        {
            if (validationResult.IsValid)
                return;

            var details = string.Join("; ", validationResult.Errors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}"));
            throw new RpcException(new Status(StatusCode.InvalidArgument, details));
        }
    }
}