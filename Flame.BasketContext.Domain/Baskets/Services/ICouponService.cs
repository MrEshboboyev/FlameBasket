﻿using Flame.BasketContext.Domain.Coupons;

namespace Flame.BasketContext.Domain.Baskets.Services;

public interface ICouponService
{
    Task<decimal> ApplyDiscountAsync(Id<Coupon> couponId, decimal totalAmount);
    Task<bool> IsActive(Id<Coupon> couponId);
}