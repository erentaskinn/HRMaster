using FluentValidation;
using IK_Project.UI.Areas.CompanyManager.Models.Departmant;

namespace IK_Project.UI.FluentValidations.CompanyManagerAreaValidator.DepartmantValidator
{
    public class DepartmantCreateValidator : AbstractValidator<CompanyManagerDepartmantCreateVM>
    {
        public DepartmantCreateValidator()
        {
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Departmant Name is required.")
           .NotNull().WithMessage("Departmant Name is required.")
           .MinimumLength(3).WithMessage("Departmant Name must be at least 3 characters.")
           .MaximumLength(255).WithMessage("Departmant Name cannot exceed 255 characters.");
        }
    }
}
