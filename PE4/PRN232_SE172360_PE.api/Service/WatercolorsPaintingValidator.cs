using FluentValidation;
using Repo.Entities;

namespace Service
{
    public class WatercolorsPaintingValidator : AbstractValidator<WatercolorsPainting>
    {
        public WatercolorsPaintingValidator()
        {
            RuleFor(x => x.PaintingName)
                .NotEmpty().WithMessage("This field is required.")
                .Length(2, 80).WithMessage("The input length is invalid.")
                .Matches(@"^([A-Z][a-z0-9@#]*\s?)+$")
                .WithMessage("Format is invalid.");

            RuleFor(x => x.PaintingAuthor)
                .NotEmpty().WithMessage("This field is required.")
                .Length(2, 80).WithMessage("The input length is invalid.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("The value must be greater than zero.");
        }
    }
}
