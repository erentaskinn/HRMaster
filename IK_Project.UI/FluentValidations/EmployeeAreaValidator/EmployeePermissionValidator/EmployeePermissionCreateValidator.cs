using FluentValidation;
using IK_Project.UI.Areas.Employee.Models.PermissionVMs;

namespace IK_Project.UI.FluentValidations.EmployeeAreaValidator.EmployeePermissionValidator
{
    public class EmployeePermissionCreateValidator : AbstractValidator<PermissionCreateVM>
    {
        public EmployeePermissionCreateValidator()
        {
            RuleFor(x => x.StartDate)
.NotEmpty().WithMessage("*Start Date is required.")
.LessThanOrEqualTo(x => x.EndDate).WithMessage("Start Date must be less than or equal to End Date.")
.GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start Date cannot be earlier than today.");
            RuleFor(x => x.EndDate)
           .NotEmpty().WithMessage("*End Date is required.")
           .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End Date must be greater than or equal to Start Date.");
        }
    }
}
