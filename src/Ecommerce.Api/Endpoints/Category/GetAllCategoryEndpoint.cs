using Ecommerce.Application.Features.Category.Queries;
using MediatR;

namespace Ecommerce.Api.Endpoints.Category
{
    public class GetAllCategoryEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/categories", async (IMediator mediator) =>
            {
                var response = await mediator.Send(new GetAllCategoriesQuery());
                return Results.Ok(response);
            })
            .WithTags("Category")
            .WithSummary("Get all category")
            .Produces<GetAllCategoryResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
