namespace Flame.BasketContext.Domain.Baskets.Events;

public class BasketItemActivatedEvent(
    Id<Basket> basketId,
    BasketItem basketItem) : BaseBasketDomainEvent(basketId.Value)
{
    public BasketItem BasketItem { get; } = basketItem;
}