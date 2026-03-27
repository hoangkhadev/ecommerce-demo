using Ecommerce.Application.Features.Product.Queries;
using MediatR;

namespace Ecommerce.Api.Endpoints.Product
{
    public class GetProductByIdEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/products/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var response = await mediator.Send(new GetProductByIdQuery(id));
                return Results.Ok(response);
            }).WithTags("Product")
            .WithSummary("Get product detail")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
