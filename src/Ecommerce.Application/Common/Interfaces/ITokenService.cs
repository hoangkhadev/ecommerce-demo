using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string userName, UserRole userRole);
    }
}
