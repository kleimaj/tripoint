﻿using FluentValidation;
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
            .NotEmpty().Length(13, 18).WithMessage("Phone number is not a valid number.");
        RuleFor(x => x.Offer);
        RuleFor(x => x.LoanAmount).NotEqual("Loan Amount").WithMessage("Please select a loan amount.");
    }
}