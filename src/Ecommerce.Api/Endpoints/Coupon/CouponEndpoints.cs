using Ecommerce.Application.Features.Coupon.Command.ApplyCoupon;
using Ecommerce.Application.Features.Coupon.Command.CreateCoupon;
using MediatR;
using System.Security.Claims;

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

            app.MapPost("/api/v1/cart/apply-coupon", async (ApplyCouponCommand command, ISender sender, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var responses = await sender.Send(command with { UserId = Guid.Parse(userId!) });
                return Results.Ok(responses);
            }).WithTags("Coupon")
           .WithSummary("Apply coupon")
           .RequireAuthorization()
           .Produces<CouponCalculationResponse>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
