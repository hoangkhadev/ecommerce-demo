using Ecommerce.Application.Features.Product.Queries;
using MediatR;

namespace Ecommerce.Api.Endpoints.Product
{
    public class GetAllProductEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/products", async ([AsParameters] GetAllProductQuery query, IMediator mediator) =>
            {
                var response = await mediator.Send(query);
                return Results.Ok(response);
            }).WithTags("Product")
            .WithSummary("Get all product")
            .Produces<GetAllProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
