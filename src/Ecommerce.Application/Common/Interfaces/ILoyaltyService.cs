using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Common.Interfaces
{
    public interface ILoyaltyService
    {
        Task ProcessOrderLoyaltyAsync(Guid userId, decimal orderAmount, CancellationToken cancellationToken);
        Task RestoreStockAsync(List<OrderItem> orderItems, CancellationToken cancellationToken);
    }
}
