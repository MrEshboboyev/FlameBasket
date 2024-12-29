namespace Flame.BasketContext.Domain.Baskets.Events;

public sealed class CustomerAssignedEvent(
    Id<Basket> basketId,
    Customer customer) : BaseBasketDomainEvent(basketId.Value)
{
    public Customer Customer { get; } = customer;
    
}