using Ecommerce.Domain.Entities;
using FluentValidation;

namespace Ecommerce.Application.Features.Coupon.Command.CreateCoupon
{
    public class CreateCouponValidator : AbstractValidator<CreateCouponCommand>
    {
        private string validTypes = string.Join(", ", Enum.GetNames(typeof(DisCountType)));
        public CreateCouponValidator()
        {

            RuleFor(x => x.DisCountType).NotEmpty().Must(BeAValidDiscountType)
                .WithMessage($"Invalid discount type. Allowed values are: {validTypes}."); ;

            RuleFor(x => x.Value)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Value)
                .LessThanOrEqualTo(100)
                .When(x => x.DisCountType == nameof(DisCountType.PERCENTAGE));

            RuleFor(x => x.MinOrderValue).NotNull().GreaterThanOrEqualTo(0);

            RuleFor(x => x.StartDate).NotEmpty()
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddMinutes(-1));

            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThan(x => x.StartDate);

            RuleFor(x => x.UsageLimit).NotNull().GreaterThanOrEqualTo(0);
        }

        private bool BeAValidDiscountType(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return false;
            return Enum.TryParse(typeof(DisCountType), type, true, out _);
        }
    }
}
