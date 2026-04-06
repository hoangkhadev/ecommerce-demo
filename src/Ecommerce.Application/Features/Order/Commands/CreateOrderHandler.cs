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
                    decimal totalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);
                    decimal discountAmount = 0;

                    // Check stock
                    foreach (var item in cartItems)
                    {
                        if (item.Product.Stock < item.Quantity)
                        {
                            throw new InvalidOperationException($"Product '{item.Product.Name}' is out of stock (Available: {item.Product.Stock})");
                        }
                        item.Product.Stock -= item.Quantity;
                    }

                    if (!string.IsNullOrEmpty(cart.AppliedCouponCode))
                    {
                        var coupon = await context.Coupons.FirstOrDefaultAsync(
                            x => x.Code == cart.AppliedCouponCode && x.UsageLimit >= 0, cancellationToken);

                        if (coupon == null || coupon.UsageLimit <= 0
                        || DateTime.UtcNow < coupon.StartDate || DateTime.UtcNow > coupon.EndDate
                        || totalAmount < coupon.MinOrderValue)
                        {
                            throw new InvalidOperationException("Coupon is invalid, expired or minimum value not met.");
                        }

                        discountAmount = coupon.DiscountType == nameof(DisCountType.PERCENTAGE)
                        ? totalAmount * (coupon.Value / 100)
                        : coupon.Value;

                        if (discountAmount > totalAmount) discountAmount = totalAmount;

                        coupon.UsageLimit -= 1;
                    }

                    var order = new OrderEntity
                    // Create order
                    {
                        UserId = request.UserId,
                        TotalAmount = totalAmount,
                        DiscountAmount = discountAmount,
                        FinalAmount = totalAmount - discountAmount,
                        AppliedCouponCode = cart.AppliedCouponCode,
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
                    cart.AppliedCouponCode = null;

                    try
                    {
                        await context.SaveChangesAsync(cancellationToken);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw new InvalidOperationException("Data was modified by another user. Please try again.");
                    }

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
