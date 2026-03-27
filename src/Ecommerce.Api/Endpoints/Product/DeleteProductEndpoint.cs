using Ecommerce.Application.Features.Product.Commands;
using MediatR;

namespace Ecommerce.Api.Endpoints.Product
{
    public class DeleteProductEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/v1/products/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteProductCommand(id));
                return Results.NoContent();
            }).WithTags("Product")
            .WithSummary("Soft delete product")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
