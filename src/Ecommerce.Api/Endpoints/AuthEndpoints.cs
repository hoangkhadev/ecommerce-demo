using Ecommerce.Application.Features.Auth.Commands;
using FluentValidation;
using MediatR;

namespace Ecommerce.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/auth").WithTags("Auth");

            group.MapPost("/register", async (RegisterCommand cmd, IMediator mediator) =>
            {
                try
                {
                    var response = await mediator.Send(cmd);
                    return Results.Created("", response);
                }
                catch (ValidationException ex)
                {
                    var errors = ex.Errors.Select(x => new { x.PropertyName, x.ErrorMessage });
                    return Results.BadRequest((object)new { Errors = errors });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { Error = ex.Message });
                }
            })
            .WithSummary("Register new user account")
            .Produces<RegisterResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
