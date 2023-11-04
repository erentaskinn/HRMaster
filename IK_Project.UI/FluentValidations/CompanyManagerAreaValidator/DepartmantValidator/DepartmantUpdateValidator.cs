using FluentValidation;
using IK_Project.UI.Areas.CompanyManager.Models.AdvanceVM;
using IK_Project.UI.Areas.CompanyManager.Models.Departmant;

namespace IK_Project.UI.FluentValidations.CompanyManagerAreaValidator.DepartmantValidator
{
    public class DepartmantUpdateValidator : AbstractValidator<CompanyManagerDepartmantUpdateVM>
    {
        public DepartmantUpdateValidator()
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("Departmant Name is required.").NotEmpty().WithMessage("Departmant Name is required.").MinimumLength(3).WithMessage("It must be at least 3 characters.")
                        .MaximumLength(255).WithMessage("Company name cannot exceed 255 characters.");
        }
    }
}
