namespace Flame.BasketContext.Domain.Baskets.Events;

public sealed class BasketItemAddedEvent(
    Id<Basket> basketId,
    BasketItem basketItem) : BaseBasketDomainEvent(basketId.Value)
{
    public BasketItem BasketItem { get; } = basketItem;
}