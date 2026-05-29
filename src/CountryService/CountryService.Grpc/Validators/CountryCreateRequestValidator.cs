namespace CountryService.Grpc.Validators;

public sealed class CountryCreateRequestValidator : AbstractValidator<CountryCreateRequest>
{
    public CountryCreateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(200);
        RuleFor(x => x.CapitalCity).MaximumLength(50);
        RuleFor(x => x.Anthem).MaximumLength(200);
        RuleFor(x => x.FlagUri).MaximumLength(200)
            .Must(BeEmptyOrValidAbsoluteUri).WithMessage("FlagUri must be a valid absolute uri");
        RuleFor(x => x.Languages).NotEmpty()
            .Must(HaveUniqueValues).WithMessage("Languages must not contain duplicates");
        RuleForEach(x => x.Languages)
            .GreaterThan(0).WithMessage("Language must not be empty");
    }

    private static bool BeEmptyOrValidAbsoluteUri(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
               || Uri.TryCreate(value, UriKind.Absolute, out _);
    }

    private static bool HaveUniqueValues(IEnumerable<int> values)
    {
        var array = values.ToArray();
        return array.Distinct().Count() == array.Length;
    }
}