using FluentValidation;
using NewHavenLanding.Models;

namespace NewHavenLanding.Validators;

public class CustomerLoanInfoValidator : AbstractValidator<CustomerLoanInfo> {
    public CustomerLoanInfoValidator() {
        RuleFor(x => x.LoanType)
            .NotEmpty();
        RuleFor(x => x.IsCurrentWithCreditors)
            .NotEmpty();
        RuleFor(x => x.IsEmployed)
            .NotEmpty();
        RuleFor(x => x.MonthlyIncome)
            .NotEmpty();
        RuleFor(x => x.EstimatedDebt)
            .NotEmpty();
        RuleFor(x => x.LanguagePreference)
            .NotEmpty();
       // RuleFor(x=>x.Customer)
    }
}