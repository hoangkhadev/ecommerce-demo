using Ecommerce.Application.Features.Category.Commands;
using MediatR;

namespace Ecommerce.Api.Endpoints.Category
{
    public class UpdateCategoryEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/v1/categories/{id:guid}", async (Guid id, UpdateCategoryCommand cmd, IMediator mediator) =>
            {
                var updatedCommand = cmd with { Id = id };
                var response = await mediator.Send(updatedCommand);

                return Results.Ok(response);
            }).WithTags("Category")
            .WithSummary("Update category")
            .RequireAuthorization("AdminOnly")
            .Produces<UpdateCategoryResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
