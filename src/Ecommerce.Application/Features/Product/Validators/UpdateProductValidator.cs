using Ecommerce.Application.Features.Product.Commands;
using FluentValidation;

namespace Ecommerce.Application.Features.Product.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        }
    }
}
