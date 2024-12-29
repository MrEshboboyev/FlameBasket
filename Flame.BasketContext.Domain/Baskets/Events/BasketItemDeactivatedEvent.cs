namespace Flame.BasketContext.Domain.Baskets.Events;

public sealed class BasketItemDeactivatedEvent(
    Id<Basket> basketId,
    BasketItem basketItem)
    : BaseBasketDomainEvent(basketId.Value)
{
    public BasketItem BasketItem { get; } = basketItem;
}