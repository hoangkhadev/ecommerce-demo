using Ecommerce.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Services
{
    public class CouponService(IApplicationDbContext context) : ICouponService
    {
        private const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        public async Task<string> GenerateUniqueCode(string prefix = "CP", int length = 6, CancellationToken cancellationToken = default)
        {
            string code;
            bool isExists;

            do
            {
                var random = new Random();
                //var randomPart = new string(Enumerable.Repeat(AllowedChars, length)
                //    .Select(s => s[random.Next(s.Length)]).ToArray());
                var randomPart = string.Create(length, AllowedChars, (span, chars) =>
                {
                    for (int i = 0; i < span.Length; i++)
                    {
                        span[i] = chars[Random.Shared.Next(chars.Length)];
                    }
                });

                code = $"{prefix}-{randomPart}".ToUpper();

                isExists = await context.Coupons.AnyAsync(x => x.Code == code, cancellationToken);

            } while (isExists);

            return code;
        }
    }
}
