using FluentValidation;
using TriPoint.Models;

namespace TriPoint.Validators;

public class ShortCustomerValidator : AbstractValidator<ShortCustomer> {
    public ShortCustomerValidator() {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Enter a valid email address.");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Cell phone number is required").Length(10, 15).WithMessage("Cell phone number is not a valid number.").Matches(@"^[0-9*)(#+-/\s]+$").WithMessage("Cell phone number is not a valid number.");
        RuleFor(x => x.Offer);
        RuleFor(x => x.LoanAmount).NotEmpty().WithMessage("Please enter a loan amount.");
    }
}