namespace CountryService.Grpc.Validators;

public class CountryIdRequestValidator : AbstractValidator<CountryIdRequest>
{
    public CountryIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Country Id must be greater than 0");
    }
}