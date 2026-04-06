using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Features.Product.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Review.Queries
{
    public class GetReviewByProductIdHandler(IApplicationDbContext context) : IRequestHandler<GetReviewByProductIdQuery, GetReviewByProductIdResponse>
    {
        public async Task<GetReviewByProductIdResponse> Handle(GetReviewByProductIdQuery request, CancellationToken cancellationToken)
        {
            var query = context.Reviews.AsNoTracking().Where(r => r.ProductId == request.ProductId);
            int totalRecords = await query.CountAsync(cancellationToken);
            if (totalRecords == 0)
            {
                return new GetReviewByProductIdResponse(0, new PageResult<ReviewDto>([], 0, request.PageNumber, request.PageSize));
            }
            var averageRating = await query.AverageAsync(r => r.Rating, cancellationToken);

            var items = await query.OrderByDescending(r => r.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    UserName = r.User.Username,
                    CreatedAt = r.CreatedAt
                }).ToListAsync(cancellationToken);

            return new GetReviewByProductIdResponse(Math.Round(averageRating, 1),
                new PageResult<ReviewDto>(items, totalRecords, request.PageNumber, request.PageSize));
        }
    }
}
