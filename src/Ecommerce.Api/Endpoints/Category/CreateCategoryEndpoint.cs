using Ecommerce.Application.Features.Category.Commands;
using MediatR;

namespace Ecommerce.Api.Endpoints.Category
{
    public class CreateCategoryEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/categories", async (CreateCategoryCommand cmd, IMediator mediator) =>
            {
                var response = await mediator.Send(cmd);
                return Results.Created("", response);
            }).WithTags("Category")
            .WithSummary("Create new category")
            .RequireAuthorization("AdminOnly")
            .Produces<CreateCategoryResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
