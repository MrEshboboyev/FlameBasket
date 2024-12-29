using Flame.BasketContext.Domain.Common;

namespace Flame.BasketContext.Domain.Baskets.Events;

[AggregateType(BasketEventConstants.BasketsAggregateTypeName)]
public abstract class BaseBasketDomainEvent(
    Guid aggregateId,
    DateTimeOffset? occurredOnUtc = null)
    : DomainEvent(
        aggregateId,
        occurredOnUtc ?? DateTimeOffset.UtcNow)
{ }