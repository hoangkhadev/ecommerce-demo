using Ecommerce.Application.Features.Category.Commands;
using MediatR;

namespace Ecommerce.Api.Endpoints.Category
{
    public class DeleteCategoryEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/v1/categories/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteCategoryCommand(id));
                return Results.NoContent();
            }).WithTags("Category")
            .WithSummary("Delete category")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
