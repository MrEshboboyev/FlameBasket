namespace Flame.BasketContext.Domain.Coupons.Events;

public sealed class CouponDeactivatedEvent(
    Id<Coupon> id) : BaseCouponDomainEvent(id);