using FluentValidation;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.FluentValidations.AdminAreaValidator;

namespace IK_Project.UI.FluentValidations.CompanyManagerAreaValidator.EmployeeValidator
{
    public class EmployeeUpdateValidator : AbstractValidator<CompanyManagerEmployeeUpdateVM>
    {
        public EmployeeUpdateValidator()
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
            RuleFor(x => x.LogoFile).SetValidator(new ImageValidator()!).When(model => model.LogoFile != null);

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
    

