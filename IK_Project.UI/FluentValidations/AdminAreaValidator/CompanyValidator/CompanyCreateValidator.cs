using FluentValidation;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyVM;

namespace IK_Project.UI.FluentValidations.AdminAreaValidator.CompanyValidator
{
    public class CompanyCreateValidator: AbstractValidator<AdminCompanyCreateVM>
    {
        public CompanyCreateValidator()
        {
            RuleFor(x => x.CompanyName)
                        .NotNull().WithMessage("The company name is required.")
                        .NotEmpty().WithMessage("The company name is required.")
                        .MinimumLength(3).WithMessage("It must be at least 3 characters.")
                        .MaximumLength(255).WithMessage("Company name cannot exceed 255 characters.");
            RuleFor(x=>x.TaxNo)
            .NotEmpty().WithMessage("Tax number is required.")
            .Length(10).WithMessage("Tax number must be 10 digits long.")
            .Matches("^[0-9]+$").WithMessage("Tax number should consist of only digits.");
           
            RuleFor(vm => vm.Address)
           .NotEmpty().WithMessage("Address is required.")
           .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");
            RuleFor(vm => vm.Email)
           .NotEmpty().WithMessage("Email is required.")
           .EmailAddress().WithMessage("Invalid email address format.").NotNull().WithMessage("The company Address is required.")
           .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");
            RuleFor(vm => vm.NumberOfEmployees)
            .NotEmpty().WithMessage("Number of employees is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Number of employees must be a non-negative value.");
            RuleFor(vm => vm.DealStartDate)
            .NotEmpty().WithMessage("Deal start date is required.").GreaterThanOrEqualTo(DateTime.Today).WithMessage("Deal start date must be today or a future date.");
            //RuleFor(vm => vm.DealEndDate)
            //.NotEmpty().WithMessage("Deal end date is required.")
            //.GreaterThanOrEqualTo(vm => vm.DealStartDate).WithMessage("Deal end date cannot be earlier than deal start date.");
            RuleFor(x=>x.LogoFile).SetValidator(new ImageValidator()!).When(model=>model.LogoFile !=null);
            RuleFor(vm => vm.PhoneNumber)
    .NotEmpty().WithMessage("Phone number is required.")
    .Matches(@"^(05\d{9})$").WithMessage("Phone number must be in the format 05xxxxxxxxx.");
        }

    }
}
