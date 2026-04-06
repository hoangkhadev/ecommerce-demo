using FluentValidation;

namespace Ecommerce.Application.Features.Review.Command
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.Rating).NotNull()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(5);

            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Comment).NotEmpty();
        }
    }
}
