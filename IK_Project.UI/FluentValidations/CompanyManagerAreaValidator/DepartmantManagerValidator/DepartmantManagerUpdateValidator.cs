using FluentValidation;
using IK_Project.UI.Areas.CompanyManager.Models.DepartmanManager;
using IK_Project.UI.FluentValidations.AdminAreaValidator;

namespace IK_Project.UI.FluentValidations.CompanyManagerAreaValidator.DepartmantManagerValidator
{
    public class DepartmantManagerUpdateValidator : AbstractValidator<CompanyManagerDepartmantManagerUpdateVM>
    {
        public DepartmantManagerUpdateValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("*Name is required.")
            .NotNull().WithMessage("*Name is required.")
            .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("*LastName is required.")
                .NotNull().WithMessage("*LastName is required.")
                .MaximumLength(255).WithMessage("LastName cannot exceed 255 characters.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("*Birth Date is required.")
                .NotNull().WithMessage("*Birth Date cannot be null.")
                .Must(BeAValidDate).WithMessage("You must be at least 18 years old.");

            RuleFor(x => x.BirhtPlace)
                .NotEmpty().WithMessage("*Birth Place is required.")
                .MaximumLength(255).WithMessage("Birth Place cannot exceed 255 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("*Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");





            RuleFor(x => x.StartDate)
    .NotEmpty().WithMessage("*Start Date is required.")
    .NotNull().WithMessage("*Start Date cannot be null.")
    .LessThanOrEqualTo(DateTime.Today).WithMessage("Start Date cannot be greater than today.");
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
