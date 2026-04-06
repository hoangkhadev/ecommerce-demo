using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Coupon.Command.CreateCoupon;

public record CreateCouponCommand(
    string DisCountType,
    decimal Value,
    decimal MinOrderValue,
    DateTime StartDate,
    DateTime EndDate,
    int UsageLimit
) : IRequest<Guid>;
