
using Ecommerce.Application.Features.Cart.Commands;
using FluentValidation;

namespace Ecommerce.Application.Features.Cart.Validators
{
    public class UpdateValidator : AbstractValidator<UpdateCartCommand>
    {
        private readonly string[] _validTypes = { "increase", "decrease", "remove" };
        public UpdateValidator()
        {
            RuleFor(x => x.Type)
            .NotEmpty()
            .Must(type => _validTypes.Contains(type)).WithMessage("Type must be one of: 'increase', 'decrease' or 'remove'.");
        }
    }
}
