using Ecommerce.Application.Features.Order.Commands;
using Ecommerce.Application.Features.Order.Queries;
using MediatR;
using System.Security.Claims;

namespace Ecommerce.Api.Endpoints.Order
{
    public class OrderEndpoints : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/orders").WithTags("Order").RequireAuthorization();

            group.MapPost("/checkout", async (IMediator mediator, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await mediator.Send(new CreateOrderCommand(Guid.Parse(userId!)));

                return Results.Created("", response);
            }).WithSummary("Create order from cart")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/", async (IMediator mediator, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await mediator.Send(new GetUserOrderQuery(Guid.Parse(userId!)));
                return Results.Ok(response);
            }).WithSummary("Get current user order")
            .Produces<GetUserOrderResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
