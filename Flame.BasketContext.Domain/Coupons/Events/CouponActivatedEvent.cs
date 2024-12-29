namespace Flame.BasketContext.Domain.Coupons.Events;

public sealed class CouponActivatedEvent(
    Id<Coupon> id) : BaseCouponDomainEvent(id);