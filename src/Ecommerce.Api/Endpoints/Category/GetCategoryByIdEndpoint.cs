using Ecommerce.Application.Features.Category.Queries;
using Ecommerce.Domain.Entities;
using MediatR;
using System.Security.Claims;

namespace Ecommerce.Api.Endpoints.Category
{
    public class GetCategoryByIdEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/categories/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var response = await mediator.Send(new GetCategoryByIdQuery(id));
                return Results.Ok(response);
            }).WithTags("Category")
            .WithSummary("Get detail category")
            .Produces<GetCategoryByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
