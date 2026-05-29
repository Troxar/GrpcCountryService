namespace CountryService.Grpc.Extensions;

public static class ValidatorExtensions
{
    extension<T>(IValidator<T> validator)
    {
        public async Task ValidateAndThrowRpcExceptionsAsync(T instance, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(instance, cancellationToken);
            result.ThrowIfInvalid();
        }
    }
}