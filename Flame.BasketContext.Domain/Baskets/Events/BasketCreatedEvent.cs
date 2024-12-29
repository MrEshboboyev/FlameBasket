namespace Flame.BasketContext.Domain.Baskets.Events;

public class BasketCreatedEvent(
    Id<Basket> basketId,
    Guid customerId) 
    : BaseBasketDomainEvent(aggregateId: basketId.Value)
{
    public Guid CustomerId { get; } = customerId;
}