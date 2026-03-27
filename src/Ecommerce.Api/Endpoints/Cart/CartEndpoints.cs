using Ecommerce.Application.Features.Cart.Commands;
using Ecommerce.Application.Features.Cart.Queries;
using MediatR;
using System.Security.Claims;

namespace Ecommerce.Api.Endpoints.Cart
{
    public class CartEndpoints : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/carts").WithTags("Cart").RequireAuthorization();

            group.MapPost("/", async (AddToCartCommand cmd, IMediator mediator, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var response = await mediator.Send(cmd with { UserId = Guid.Parse(userId!) });
                return Results.Ok(response);
            }).WithSummary("Add product to cart")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/", async (IMediator mediator, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var response = await mediator.Send(new GetCartQuery(Guid.Parse(userId!)));
                return Results.Ok(response);
            }).WithSummary("Get user cart")
            .Produces<GetCartResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPatch("/items/{productId:guid}", async (Guid productId, UpdateCartCommand cmd, IMediator mediator, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var response = await mediator.Send(cmd with { ProductId = productId, UserId = Guid.Parse(userId!) });
                return Results.Ok(response);
            }).WithSummary("Update item from cart")
            .Produces<UpdateCartResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
