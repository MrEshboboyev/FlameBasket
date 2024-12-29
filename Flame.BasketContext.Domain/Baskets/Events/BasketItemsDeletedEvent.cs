namespace Flame.BasketContext.Domain.Baskets.Events;

public sealed class BasketItemsDeletedEvent(
    Id<Basket> basketId) : BaseBasketDomainEvent(basketId.Value);