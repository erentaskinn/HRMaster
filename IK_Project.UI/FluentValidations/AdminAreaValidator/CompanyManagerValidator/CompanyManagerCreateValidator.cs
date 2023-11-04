using FluentValidation;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyManagerVMs;
using System;

namespace IK_Project.UI.FluentValidations.AdminAreaValidator.CompanyManagerValidator
{
    public class AdminCompanyManagerCreateVMValidator : AbstractValidator<AdminCompanyManagerCreateVM>
    {
        public AdminCompanyManagerCreateVMValidator()
        {
            RuleFor(x => x.Name)
.NotEmpty().WithMessage("Name is required.")
.NotNull().WithMessage("Name cannot be null.")
.Length(3, 255).WithMessage("Name must be between 3 and 255 characters.");

            RuleFor(x => x.LastName)
               .NotEmpty().WithMessage("Last Name is required.")
               .NotNull().WithMessage("Last Name cannot be null.")
               .Length(3, 255).WithMessage("Last Name must be between 3 and 255 characters.");


            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth Date is required.").LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Birthdate cannot be greater than today.")
                .Must(BeAValidDate).WithMessage("You must be at least 18 years old.");

            RuleFor(x => x.BirhtPlace)
               .NotEmpty().WithMessage("Birth Place is required.")
               .NotNull().WithMessage("Birth Place cannot be null.")
               .Length(2, 255).WithMessage("Birth Place must be between 3 and 255 characters.");

            //RuleFor(x => x.Email)
            //    .NotEmpty().WithMessage("Email is required.")
            //    .EmailAddress().WithMessage("Invalid email address format.")
            //    .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.").NotNull().WithMessage("The companyManager Address is required.")
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Matches(@"^(05\d{9})$").WithMessage("Phone Number must be in the format 05xxxxxxxxx.");

            RuleFor(x => x.LogoFile).SetValidator(new ImageValidator()!).When(model => model.LogoFile != null);




            //RuleFor(x => x.CompanyID)
            //    .NotEmpty().WithMessage("Company selection is required.");

        }

        private bool BeAValidDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return false; 
            }

            var today = DateTime.Today;
            var age = today.Year - date.Value.Year;

            if (date.Value.Date > today.AddYears(-age))
            {
                age--;
            }

            return age >= 18;
        }
    }
}

