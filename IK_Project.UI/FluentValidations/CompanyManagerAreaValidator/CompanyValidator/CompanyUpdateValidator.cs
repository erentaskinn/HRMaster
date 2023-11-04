using FluentValidation;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyVM;
using IK_Project.UI.FluentValidations.AdminAreaValidator;

namespace IK_Project.UI.FluentValidations.CompanyManagerAreaValidator.CompanyValidator
{
    public class CompanyUpdateValidator : AbstractValidator<CompanyManagerCompanyUpdateVM>
    {
        public CompanyUpdateValidator()
        {
            

           
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(05\d{9})$").WithMessage("Phone number must be in the format 05xxxxxxxxx.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

            RuleFor(x => x.NumberOfEmployees)
                .NotEmpty().WithMessage("Number of employees is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Number of employees must be a non-negative value.");

            
            RuleFor(x => x.LogoFile).SetValidator(new ImageValidator()!).When(model => model.LogoFile != null);
        }
    }

}
