using MediatR;
using System.Text.Json.Serialization;

namespace Ecommerce.Application.Features.Coupon.Command.ApplyCoupon;

public record CouponCalculationResponse
(
    string CouponCode,
    decimal DiscountAmount,
    decimal FinalTotal
);
public record ApplyCouponCommand([property: JsonIgnore] Guid UserId, string CouponCode) : IRequest<CouponCalculationResponse>;
