namespace Flame.BasketContext.Domain.Baskets.Events;

public sealed class BasketItemsAmountCalculatedEvent(
    Id<Basket> basketId,
    decimal amount)
    : BaseBasketDomainEvent(basketId.Value)
{
    public decimal Amount { get; } = amount;
}