using FluentValidation;
using IK_Project.UI.Areas.CompanyManager.Models.DepartmanManager;
using IK_Project.UI.FluentValidations.AdminAreaValidator;

namespace IK_Project.UI.FluentValidations.CompanyManagerAreaValidator.DepartmantManagerValidator
{
    public class DepartmantManagerCreateValidator : AbstractValidator<CompanyManagerDepartmantManagerCreateVM>
    {
        public DepartmantManagerCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("*Name is required.,,")
                .NotNull().WithMessage("*Name cannot be null.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("*Last Name is required.")
                .NotNull().WithMessage("*Last Name cannot be null.")
                .MaximumLength(255).WithMessage("Last Name cannot exceed 255 characters.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("*Birth Date is required.")
                .NotNull().WithMessage("*Birth Date cannot be null.")
                .Must(BeAValidDate).WithMessage("You must be at least 18 years old.");

            RuleFor(x => x.BirhtPlace)
                .NotEmpty().WithMessage("*Birth Place is required.")
                .NotNull().WithMessage("*Birth Place cannot be null.")
                .MaximumLength(255).WithMessage("Birth Place cannot exceed 255 characters.");
            RuleFor(x => x.StartDate)
    .NotEmpty().WithMessage("*Start Date is required.")
    .NotNull().WithMessage("*Start Date cannot be null.")
    .LessThanOrEqualTo(DateTime.Today).WithMessage("Start Date cannot be greater than today.");
            //RuleFor(x => x.Email)
            //    .NotEmpty().WithMessage("*Email is required.")
            //    .NotNull().WithMessage("*Email cannot be null.")
            //    .EmailAddress().WithMessage("Invalid email address format.")
            //    .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("*Address is required.")
                .NotNull().WithMessage("*Address cannot be null.")
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("*Phone Number is required.")
                .NotNull().WithMessage("*Phone Number cannot be null.")
                .Matches(@"^(05\d{9})$").WithMessage("Phone Number must be in the format 05xxxxxxxxx.");

            //RuleFor(x => x.CompanyID)
            //    .NotEmpty().WithMessage("Company selection is required.")
            //    .NotNull().WithMessage("Company selection cannot be null.");
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
