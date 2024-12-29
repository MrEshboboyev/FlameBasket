namespace Flame.BasketContext.Domain.Baskets.Events;

public sealed class BasketItemCountUpdatedEvent(
    Id<Basket> basketId,
    BasketItem basketItem, 
    int count)
    : BaseBasketDomainEvent(basketId.Value)
{
    public BasketItem BasketItem { get; } = basketItem;
    public int Count { get; } = count;
}