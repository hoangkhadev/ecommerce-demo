using Ecommerce.Application.Features.Coupon.Command.CreateCoupon;
using MediatR;

namespace Ecommerce.Api.Endpoints.Coupon
{
    public static class CouponEndpoints
    {
        public static void MapCouponEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/amdin/coupons", async (CreateCouponCommand command, ISender sender) =>
            {
                var responses = await sender.Send(command);
                return Results.Created("", responses);
            }).WithTags("Coupon")
            .WithSummary("Create new coupon")
            .RequireAuthorization("AdminOnly")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
