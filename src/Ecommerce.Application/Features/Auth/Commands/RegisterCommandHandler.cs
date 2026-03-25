using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Auth.Commands
{
    public record RegisterResponse(string Message, Guid UserId);
    public record RegisterCommand(string UserName, string Email, string Password) : IRequest<RegisterResponse>;

    public class RegisterCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher) : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var exists = await context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (exists)
            {
                var failures = new List<FluentValidation.Results.ValidationFailure>
                {
                    new("Email", "Email already exists")
                };

                throw new FluentValidation.ValidationException(failures);
            }

            var passwordHash = passwordHasher.Hash(request.Password);

            var newUser = new User
            {
                Username = request.UserName,
                Email = request.Email,
                PasswordHash = passwordHash,
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync(cancellationToken);

            return new RegisterResponse("Register success", newUser.Id);
        }
    }
}
