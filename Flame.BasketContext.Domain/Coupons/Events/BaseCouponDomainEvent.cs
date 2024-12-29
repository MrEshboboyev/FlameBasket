using Flame.BasketContext.Domain.Common;

namespace Flame.BasketContext.Domain.Coupons.Events;

[AggregateType(BasketEventConstants.CouponsAggregateTypeName)]
public abstract class BaseCouponDomainEvent(
    Id<Coupon> aggregateId,
    DateTimeOffset? occurredOnUtc = null)
    : DomainEvent(
        aggregateId.Value,
        occurredOnUtc ?? DateTimeOffset.UtcNow)
{ }