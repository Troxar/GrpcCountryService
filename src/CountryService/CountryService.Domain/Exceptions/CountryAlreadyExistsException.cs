namespace CountryService.Domain.Exceptions;

public sealed class CountryAlreadyExistsException : Exception
{
    public string Name { get; }

    public CountryAlreadyExistsException(string name, Exception? innerException = null)
        : base($"Country with name '{name}' already exists", innerException)
    {
        Name = name;
    }
}