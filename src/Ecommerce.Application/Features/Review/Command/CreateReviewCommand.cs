using MediatR;
using System.Text.Json.Serialization;

namespace Ecommerce.Application.Features.Review.Command;

public record CreateReviewCommand(
   [property: JsonIgnore]
   Guid UserId,
   Guid ProductId,
   int Rating,
   string Comment
) : IRequest<Guid>;
