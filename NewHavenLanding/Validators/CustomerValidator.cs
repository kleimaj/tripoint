using FluentValidation;
using NewHavenLanding.Models;

namespace NewHavenLanding.Validators;

public class CustomerValidator : AbstractValidator<Customer> {
    public CustomerValidator() {
        RuleFor(x => x.FirstName)
            .NotEmpty();
        RuleFor(x => x.LastName)
            .NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty();
        RuleFor(x => x.Phone)
            .NotEmpty().Length(13, 18).WithMessage("Phone number is not a valid number.");
        RuleFor(x => x.Cell)
            .Length(13, 18).WithMessage("Cell phone number is not a valid number.");
        RuleFor(x => x.Address)
            .NotEmpty();
        RuleFor(x => x.City)
            .NotEmpty();
        RuleFor(x => x.State)
            .NotNull();
        RuleFor(x => x.Zip)
            .NotEmpty();
    }
}