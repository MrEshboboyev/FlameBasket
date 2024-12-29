namespace Flame.BasketContext.Domain.Baskets.Events;

public sealed class TotalAmountCalculatedEvent(
    Id<Basket> basketId,
    decimal totalAmount) : BaseBasketDomainEvent(basketId.Value)
{
    public decimal TotalAmount { get; } = totalAmount;
}