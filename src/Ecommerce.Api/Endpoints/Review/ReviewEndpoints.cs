using Ecommerce.Application.Features.Review.Command;
using Ecommerce.Application.Features.Review.Queries;
using MediatR;
using System.Security.Claims;

namespace Ecommerce.Api.Endpoints.Review
{
    public static class ReviewEndpoints
    {
        public static void MapReviewEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/reviews", async (CreateReviewCommand command, ISender sender, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var response = await sender.Send(command with { UserId = Guid.Parse(userId!) });
                return Results.Created("", response);
            }).WithTags("Review")
            .WithSummary("Create review")
            .RequireAuthorization()
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            app.MapPost("/api/v1/products/{id:guid}/reviews", async (Guid id, int? pageNumber, int? pageSize, ISender sender) =>
            {
                var response = await sender.Send(new GetReviewByProductIdQuery(id, pageNumber ?? 1, pageSize ?? 10));
                return Results.Ok(response);
            }).WithTags("Review")
          .WithSummary("Get all review by product id")
          .Produces<Guid>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
