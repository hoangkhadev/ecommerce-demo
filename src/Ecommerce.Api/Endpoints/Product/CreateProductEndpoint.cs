using Ecommerce.Application.Features.Product.Commands;
using MediatR;

namespace Ecommerce.Api.Endpoints.Product
{
    public class CreateProductEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/products", async (CreateProductCommand cmd, IMediator mediator) =>
            {
                var response = await mediator.Send(cmd);
                return Results.Created("", response);
            }).WithTags("Product")
            .WithSummary("Create new product")
            .RequireAuthorization("AdminOnly")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
