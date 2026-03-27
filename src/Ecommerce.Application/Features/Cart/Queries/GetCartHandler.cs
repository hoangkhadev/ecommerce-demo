using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Application.Features.Cart.Queries
{
    public record ProductCart(Guid Id, string Name, decimal Price);
    public record CartItem(Guid Id, int Quantity, ProductCart Product);
    public record CartResponse(Guid Id, List<CartItem> Items)
    {
        public decimal TotalPrices => Items.Sum(x => x.Quantity * x.Product.Price);
    }
    public record GetCartResponse(string Message, CartResponse Data);
    public record GetCartQuery([property: JsonIgnore] Guid UserId) : IRequest<GetCartResponse>;

    public class GetCartHandler(IApplicationDbContext context) : IRequestHandler<GetCartQuery, GetCartResponse>
    {
        public async Task<GetCartResponse> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cartData = await context.Carts
                .Where(c => c.UserId == request.UserId)
                .Select(c => new CartResponse(
                    c.Id,
                    c.CartItems.Select(ci => new CartItem(
                        ci.Id,
                        ci.Quantity,
                        new ProductCart(ci.Product.Id, ci.Product.Name, ci.Product.Price)
                    )).ToList()
                )).FirstOrDefaultAsync(cancellationToken);

            string message = "Get user cart success";
            if (cartData is null)
            {
                return new GetCartResponse(message, new CartResponse(Guid.Empty, []));
            }

            return new GetCartResponse(message, cartData);
        }
    }
}
