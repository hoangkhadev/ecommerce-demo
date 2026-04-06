using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReviewEntity = Ecommerce.Domain.Entities.Review;

namespace Ecommerce.Application.Features.Review.Command
{
    public class CreateReviewHandler(IApplicationDbContext context) : IRequestHandler<CreateReviewCommand, Guid>
    {
        public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var order = await context.Orders.Include(o => o.OrderItems).Where(o => o.UserId == request.UserId
            && o.Status == OrderStatus.Completed.ToString()
            && o.OrderItems.Any(oi => oi.ProductId == request.ProductId)).FirstOrDefaultAsync(cancellationToken)
                ?? throw new InvalidOperationException("User has not purchased this product");

            var exists = await context.Reviews.AnyAsync(r =>
            r.OrderId == order.Id && r.UserId == request.UserId && r.ProductId == request.ProductId, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException("Review exists");
            }

            var review = new ReviewEntity
            {
                OrderId = order.Id,
                ProductId = request.ProductId,
                UserId = request.UserId,
                Comment = request.Comment,
                Rating = request.Rating,
                CreatedAt = DateTime.UtcNow
            };

            context.Reviews.Add(review);
            await context.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
    }
}
