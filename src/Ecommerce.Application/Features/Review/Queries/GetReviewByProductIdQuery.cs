using Ecommerce.Application.Features.Product.Queries;
using MediatR;

namespace Ecommerce.Application.Features.Review.Queries;

public record ReviewDto
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public required string Comment { get; set; }
    public string UserName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public record GetReviewByProductIdResponse(double AverageRating, PageResult<ReviewDto> Data);
public record GetReviewByProductIdQuery(
    Guid ProductId,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<GetReviewByProductIdResponse>;
