using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Order.Commands.UpdateStatusOrder
{
    public class UpdateStatusOrderHandler(IApplicationDbContext context, ILoyaltyService loyaltyService) : IRequestHandler<UpdateStatusOrderCommand, Guid>
    {
        public async Task<Guid> Handle(UpdateStatusOrderCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await context.BeginTransactionAsync(cancellationToken);
            try
            {
                var order = await context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Order with ID {request.OrderId} not found");

                if (!IsValidTransition(order.Status, request.Status))
                    throw new Exception($"Invalid status transition from {order.Status} to {request.Status}");

                switch (request.Status)
                {
                    case nameof(OrderStatus.Completed):
                        await loyaltyService.ProcessOrderLoyaltyAsync(order.UserId, order.FinalAmount, cancellationToken);
                        break;

                    case nameof(OrderStatus.Cancelled):
                        await loyaltyService.RestoreStockAsync(order.OrderItems, cancellationToken);
                        break;
                }

                order.Status = request.Status.ToString();

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return order.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private bool IsValidTransition(string currentStatus, string nextStatus)
        {
            return (currentStatus, nextStatus) switch
            {
                ("Pending", "Approved") => true,
                ("Pending", "Cancelled") => true,
                ("Approved", "Shipped") => true,
                ("Shipped", "Completed") => true,
                _ => false
            };
        }
    }
}
