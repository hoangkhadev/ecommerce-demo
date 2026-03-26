using Ecommerce.Application.Features.Category.Commands;
using FluentValidation;

namespace Ecommerce.Application.Features.Category.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
        }
    }
}
