using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Coupon.Command.ApplyCoupon
{
    public class ApplyCouponHandler(IApplicationDbContext context) : IRequestHandler<ApplyCouponCommand, CouponCalculationResponse>
    {
        public async Task<CouponCalculationResponse> Handle(ApplyCouponCommand request, CancellationToken cancellationToken)
        {
            var cart = await context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken)
                ?? throw new KeyNotFoundException($"Cart with User ID {request.UserId} not found.");

            var cartTotal = cart.CartItems.Sum(x => x.Quantity * x.Product.Price);

            if (string.IsNullOrEmpty(request.CouponCode))
            {
                cart.AppliedCouponCode = null;
                await context.SaveChangesAsync(cancellationToken);
                return new CouponCalculationResponse(
                    request.CouponCode, 0, cartTotal
                );
            }

            var coupon = await context.Coupons.FirstOrDefaultAsync(x => x.Code == request.CouponCode, cancellationToken)
                ?? throw new KeyNotFoundException($"Coupon with code {request.CouponCode} not found.");

            if (DateTime.UtcNow < coupon.StartDate || DateTime.UtcNow > coupon.EndDate)
                throw new InvalidOperationException("Coupon is expired or not yet active.");

            if (cartTotal < coupon.MinOrderValue)
                throw new InvalidOperationException($"Minimum order value of {coupon.MinOrderValue} not met.");

            if (coupon.UsageLimit == 0)
            {
                throw new InvalidOperationException("This coupon code has reached its usage limit.");
            }

            decimal discountAmount = 0;

            if (coupon.DiscountType == DisCountType.PERCENTAGE.ToString())
            {
                discountAmount = cartTotal * (coupon.Value / 100);
            }
            else
            {
                discountAmount = coupon.Value;
            }

            if (discountAmount > cartTotal) discountAmount = cartTotal;

            cart.AppliedCouponCode = request.CouponCode;
            await context.SaveChangesAsync(cancellationToken);

            return new CouponCalculationResponse(
                request.CouponCode, discountAmount, cartTotal - discountAmount
            );
        }
    }
}
