using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Auth.Commands
{
    public record LoginResponse(string Message, string AccessToken);
    public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

    public class LoginCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, ITokenService tokenService) : IRequestHandler<LoginCommand, LoginResponse>
    {
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var accessToken = tokenService.GenerateToken(user.Id, user.Username, user.Role);

            return new LoginResponse("Login success", accessToken);
        }
    }
}
