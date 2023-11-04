using FluentValidation;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyManager;
using IK_Project.UI.FluentValidations.AdminAreaValidator;

namespace IK_Project.UI.FluentValidations.CompanyManagerAreaValidator
{
    public class CompanyManagerProfileValidator : AbstractValidator<CompanyManagerCompanyManagerUpdateVM>
    {
        public CompanyManagerProfileValidator()
        {
            RuleFor(x => x.Name)
                           .NotEmpty().WithMessage("Name is required.").NotNull().WithMessage("The Employee name is required.")
                           .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(x => x.LastName).NotNull().WithMessage("The Employee Last name is required.")
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(255).WithMessage("Last Name cannot exceed 255 characters.");
            RuleFor(vm => vm.Email)
               .NotEmpty().WithMessage("Email is required.")
               .EmailAddress().WithMessage("Invalid email address format.")
               .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");
            RuleFor(x => x.PhoneNumber)
               .NotEmpty().WithMessage("Phone Number is required.")
               .Matches(@"^(05\d{9})$").WithMessage("Phone Number must be in the format 05xxxxxxxxx.");
            RuleFor(x => x.Address)
               .NotEmpty().WithMessage("Address is required.").NotNull().WithMessage("The companyManager Address is required.")
               .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

            RuleFor(x => x.LogoFile).SetValidator(new ImageValidator()!).When(model => model.LogoFile != null);
        }
    }
}
