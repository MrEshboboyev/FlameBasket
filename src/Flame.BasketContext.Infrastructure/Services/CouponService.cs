using Flame.BasketContext.Application.Repositories;
using Flame.BasketContext.Domain.Baskets.Services;
using Flame.BasketContext.Domain.Coupons;
using Flame.Common.Domain.Primitives;

namespace Flame.BasketContext.Infrastructure.Services;

public class CouponService(ICouponRepository couponRepository) : ICouponService
{
    public async Task<decimal> ApplyDiscountAsync(
        Id<Coupon> couponId,
        decimal totalAmount)
    {
        var coupon = await couponRepository.GetByIdAsync(couponId);
        if (coupon is not { IsActive: true })
            throw new InvalidOperationException("Coupon is invalid or inactive.");

        // Assuming coupon has a percentage discount property
        var discount = (totalAmount * coupon.Amount.Value) / 100;
        return totalAmount - discount;
    }

    public async Task<bool> IsActive(Id<Coupon> couponId)
    {
        var coupon = await couponRepository.GetByIdAsync(couponId);
        return coupon is { IsActive: true };
    }
}