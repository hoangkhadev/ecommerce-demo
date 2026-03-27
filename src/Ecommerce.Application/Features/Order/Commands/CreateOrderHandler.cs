using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using OrderEntity = Ecommerce.Domain.Entities.Order;

namespace Ecommerce.Application.Features.Order.Commands
{
    public record CreateOrderResponse(string Message, Guid? Id);
    public record CreateOrderCommand([property: JsonIgnore] Guid UserId) : IRequest<CreateOrderResponse>;
    public class CreateOrderHandler(IApplicationDbContext context) : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var strategy = context.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.BeginTransactionAsync(cancellationToken);
                try
                {
                    // Get user cart
                    var cart = await context.Carts
                        .Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                        .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

                    if (cart is null || cart.CartItems is null || cart.CartItems.Count == 0)
                    {
                        throw new InvalidOperationException("Cart is empty");
                    }

                    var cartItems = cart.CartItems.ToList();

                    // Check stock
                    foreach (var item in cartItems)
                    {
                        if (item.Product.Stock < item.Quantity)
                        {
                            throw new InvalidOperationException($"Product '{item.Product.Name}' is out of stock (Available: {item.Product.Stock})");
                        }
                        await context.Products.Where(p => p.Id == item.Id && p.Stock >= item.Quantity)
                            .ExecuteUpdateAsync(s => s.SetProperty(p => p.Stock, p => p.Stock - item.Quantity), cancellationToken);
                    }

                    // Create order
                    var order = new OrderEntity
                    {
                        UserId = request.UserId,
                        TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                        OrderItems = [..cartItems.Select(ci => new OrderItem
                        {
                            ProductId = ci.ProductId,
                            Price = ci.Product.Price,
                            Quantity = ci.Quantity
                        })]
                    };

                    context.Orders.Add(order);

                    // Clear items from cart
                    context.CartItems.RemoveRange(cartItems);

                    await context.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);

                    return new CreateOrderResponse("Create order success", order.Id);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }
    }
}
