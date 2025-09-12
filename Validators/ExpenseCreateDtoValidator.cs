using ExpenseTracker.Api.Models.Dto;
using FluentValidation;

namespace ExpenseTracker.Api.Validators
{
    public class ExpenseCreateDtoValidator : AbstractValidator<ExpenseCreateDto>
    {
        public ExpenseCreateDtoValidator()
        {
            RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(200);

            RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greate then zero");

            RuleFor(x => x.Category).IsInEnum().WithMessage("Category is invalid");
        }
    }
}