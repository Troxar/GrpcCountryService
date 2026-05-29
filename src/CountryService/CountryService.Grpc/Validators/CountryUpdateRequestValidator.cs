namespace CountryService.Grpc.Validators;

public class CountryUpdateRequestValidator : AbstractValidator<CountryUpdateRequest>
{
    public CountryUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Country Id must be greater than 0");
        RuleFor(x => x.Description).MaximumLength(200);
    }
}