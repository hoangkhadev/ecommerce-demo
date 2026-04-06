using Ecommerce.Application.Common.Interfaces;
using MediatR;
using CouponEntity = Ecommerce.Domain.Entities.Coupon;

namespace Ecommerce.Application.Features.Coupon.Command.CreateCoupon
{
    public class CreateCouponHandler(IApplicationDbContext context, ICouponService couponService) : IRequestHandler<CreateCouponCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = new CouponEntity
            {
                DiscountType = request.DisCountType,
                Value = request.Value,
                MinOrderValue = request.MinOrderValue,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                UsageLimit = request.UsageLimit,
                Code = await couponService.GenerateUniqueCode("CP", 6, cancellationToken)
            };

            context.Coupons.Add(coupon);
            await context.SaveChangesAsync(cancellationToken);

            return coupon.Id;
        }
    }
}
