using FluentValidation;
using IK_Project.UI.Areas.Admin.Models.ViewModels.MenuVMs;

namespace IK_Project.UI.FluentValidations.AdminAreaValidator.MenuValidator
{
    public class MenuUpdateValidator : AbstractValidator<AdminMenuUpdateVM>
    {
        public MenuUpdateValidator()
        {
            RuleFor(vm => vm.Name)
   .NotEmpty().WithMessage("Name is required.")
   .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(vm => vm.Period)
                .NotEmpty().WithMessage("Period is required.")
                .GreaterThan(0).WithMessage("Period must be greater than 0.");

            RuleFor(vm => vm.UnitPrice)
                .NotEmpty().WithMessage("Unit Price is required.")
                .GreaterThan(0).WithMessage("Unit Price must be greater than 0.");

            RuleFor(vm => vm.UserAmount)
                .NotEmpty().WithMessage("User Amount is required.")
                .GreaterThan(0).WithMessage("User Amount must be greater than 0.");

            RuleFor(vm => vm.Currecy)
                .IsInEnum().WithMessage("Select currency unit.");
        }
    }
}
