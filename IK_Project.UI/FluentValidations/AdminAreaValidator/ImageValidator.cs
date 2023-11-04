using FluentValidation;

namespace IK_Project.UI.FluentValidations.AdminAreaValidator
{
    public class ImageValidator : AbstractValidator<IFormFile>
    {
        public ImageValidator()
        {
            RuleFor(x => x.Length).LessThanOrEqualTo(4194304).WithMessage("File size exceeds the maximum allowed (4 MB).");
            RuleFor(x => x.ContentType).Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
            .WithMessage("Please upload an image file in JPEG, JPG, or PNG format only.");
        }
    }
}
