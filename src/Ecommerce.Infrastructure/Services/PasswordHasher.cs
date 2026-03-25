using Ecommerce.Application.Common.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace Ecommerce.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => BC.HashPassword(password);
        public bool Verify(string password, string hash) => BC.Verify(password, hash);
    }
}
