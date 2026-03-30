using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CartEntity = Ecommerce.Domain.Entities.Cart;

namespace Ecommerce.Application.Features.Cart.Commands
{
    public record AddToCartResponse(string Message, Guid Id);
    public record AddToCartCommand([property: JsonIgnore] Guid UserId, Guid ProductId) : IRequest<AddToCartResponse>;
    public class AddToCartHandler(IApplicationDbContext context) : IRequestHandler<AddToCartCommand, AddToCartResponse>
    {
        public async Task<AddToCartResponse> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            // check user cart exists or create new user cart
            var userCart = await context.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

            if (userCart is null)
            {
                userCart = new CartEntity { UserId = request.UserId };
                context.Carts.Add(userCart);
            }

            // check product exists
            var product = await context.Products.Where(p => p.Id == request.ProductId)
                .Select(p => new { p.Stock, p.Name }).FirstOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID '{request.ProductId}' not found");

            // check product stock
            if (product.Stock <= 0) throw new InvalidOperationException($"Product '{product.Name} is out of stock'");

            // check product exists from cart
            var productFromCart = userCart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            if (productFromCart is not null)
            {
                if ((productFromCart.Quantity + 1) > product.Stock)
                {
                    throw new InvalidOperationException($"Only {product.Stock} items available in stock.");
                }
                else
                {
                    productFromCart.Quantity++;
                }
            }
            else
            {
                var newCartItem = new CartItem
                {
                    ProductId = request.ProductId,
                    Quantity = 1
                };
                userCart.CartItems.Add(newCartItem);
            }

            await context.SaveChangesAsync(cancellationToken);

            return new AddToCartResponse("Add product to cart success", userCart.Id);
        }
    }
}
