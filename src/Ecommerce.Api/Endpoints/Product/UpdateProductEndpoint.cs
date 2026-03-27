using Ecommerce.Application.Features.Product.Commands;
using MediatR;

namespace Ecommerce.Api.Endpoints.Product
{
    public class UpdateProductEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/v1/products/{id:guid}", async (Guid id, UpdateProductRequest request, IMediator mediator) =>
            {
                var response = await mediator.Send(new UpdateProductCommand(id, request));
                return Results.Ok(response);
            }).WithTags("Product")
            .WithSummary("Update product")
            .RequireAuthorization("AdminOnly")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
