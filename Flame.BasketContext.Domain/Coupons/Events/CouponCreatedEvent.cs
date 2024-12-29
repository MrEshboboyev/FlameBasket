namespace Flame.BasketContext.Domain.Coupons.Events;

public sealed class CouponCreatedEvent(
    Id<Coupon> couponId) : BaseCouponDomainEvent(couponId);