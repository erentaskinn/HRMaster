using FluentValidation;
using IK_Project.UI.Areas.Admin.Models.ViewModels.AdminVM;

namespace IK_Project.UI.FluentValidations.AdminAreaValidator.AdminValidator
{
    public class AdminCreateValidator : AbstractValidator<AdminAdminCreateVM>
    {
        public AdminCreateValidator()
        {
            RuleFor(vm => vm.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(vm => vm.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(255).WithMessage("Last Name cannot exceed 255 characters.");
            RuleFor(vm => vm.Email)
               .NotEmpty().WithMessage("Email is required.")
               .EmailAddress().WithMessage("Invalid email address format.")
               .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");
        }
    }
}
