namespace Flame.BasketContext.Domain.Baskets.Events;

public sealed class CouponRemovedEvent(
    Id<Basket> basketId, 
    Id<Coupon> couponId) : BaseBasketDomainEvent(basketId.Value)
{
    public Id<Coupon> CouponId { get; } = couponId;
}