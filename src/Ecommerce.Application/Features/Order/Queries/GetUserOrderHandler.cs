
using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Application.Features.Order.Queries
{
    public record OrderItem();
    public record UserOrder(Guid Id, decimal TotalAmount, string Status, DateTime CreatedAt);
    public record GetUserOrderResponse(string Message, List<UserOrder> Data);
    public record GetUserOrderQuery([property: JsonIgnore] Guid UserId) : IRequest<GetUserOrderResponse>;

    public class GetUserOrderHandler(IApplicationDbContext context) : IRequestHandler<GetUserOrderQuery, GetUserOrderResponse>
    {
        public async Task<GetUserOrderResponse> Handle(GetUserOrderQuery request, CancellationToken cancellationToken)
        {
            var userOrders = await context.Orders
                .Where(o => o.UserId == request.UserId)
                .Select(o => new UserOrder(o.Id, o.TotalAmount, o.Status, o.CreatedAt))
                .ToListAsync(cancellationToken);
            return new GetUserOrderResponse("Get user order success", userOrders);
        }
    }
}
