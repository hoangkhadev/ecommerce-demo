using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Services
{
    public class LoyaltyService(IApplicationDbContext context) : ILoyaltyService
    {
        public async Task ProcessOrderLoyaltyAsync(Guid userId, decimal orderAmount, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null) return;

            int newPoints = (int)(orderAmount / 100);
            user.TotalAccumulatedPoints += newPoints;
            user.LoyaltyPoints += newPoints;

            user.Rank = user.TotalAccumulatedPoints switch
            {
                >= 2000 => nameof(RankEnum.Dinamond),
                >= 500 => nameof(RankEnum.Gold),
                _ => nameof(RankEnum.Silver),
            };
        }

        public async Task RestoreStockAsync(List<OrderItem> orderItems, CancellationToken cancellationToken)
        {
            foreach (var orderItem in orderItems)
            {
                await context.Products.Where(p => p.Id == orderItem.ProductId)
                    .ExecuteUpdateAsync(s => s.SetProperty(
                        p => p.Stock,
                        p => p.Stock + orderItem.Quantity
                    ), cancellationToken);
            }
        }
    }
}
