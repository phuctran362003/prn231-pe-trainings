using FluentValidation;
using Repository.Entities;

namespace Service.Validator
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("This field is required.")
                .Length(2, 80).WithMessage("Value must be between 2 and 80 characters.")
                .Matches(@"^([A-Z][a-z0-9@#]*\s?)+$")
                .WithMessage("Each word must start with a capital letter and contain only valid characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Value must be greater than 0.");
        }
    }
}