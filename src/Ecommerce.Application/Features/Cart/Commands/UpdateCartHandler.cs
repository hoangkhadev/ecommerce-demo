using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Application.Features.Cart.Commands
{
    public record UpdateCartResponse(string Message, Guid Id);
    public record UpdateCartCommand(
        [property: JsonIgnore] Guid UserId,
        [property: JsonIgnore] Guid ProductId,
        string Type
    ) : IRequest<UpdateCartResponse>;
    public class UpdateCartHandler(IApplicationDbContext context) : IRequestHandler<UpdateCartCommand, UpdateCartResponse>
    {
        public async Task<UpdateCartResponse> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var cartItem = await context.Carts.Include(c => c.CartItems)
                .Where(c => c.UserId == request.UserId)
                .Select(c => c.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId))
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException($"Product with Id {request.ProductId} not found in cart");

            switch (request.Type)
            {
                case "increase":
                    cartItem.Quantity++;
                    break;
                case "decrease":
                    cartItem.Quantity--;
                    break;
                case "remove":
                    context.CartItems.Remove(cartItem);
                    break;
            }

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateCartResponse("Update cart success", cartItem.CartId);
        }
    }
}
