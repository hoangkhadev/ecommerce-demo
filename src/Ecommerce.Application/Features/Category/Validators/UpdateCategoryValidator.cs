using Ecommerce.Application.Features.Category.Commands;
using FluentValidation;

namespace Ecommerce.Application.Features.Category.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
        }
    }
}
