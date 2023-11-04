using FluentValidation;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;

namespace IK_Project.UI.FluentValidations.EmployeeAreaValidator.EmployeeExpenseValidator
{
    public class EmployeeExpenseUpdateValidator : AbstractValidator<ExpenseUpdateVM>
    {
        public EmployeeExpenseUpdateValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.")
                .InclusiveBetween(0, 100000).WithMessage("Amount must be between 0 and 100,000.");
        }
    }
}
