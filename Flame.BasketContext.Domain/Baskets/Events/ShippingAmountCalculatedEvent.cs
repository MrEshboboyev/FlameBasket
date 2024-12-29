namespace Flame.BasketContext.Domain.Baskets.Events;

public sealed class ShippingAmountCalculatedEvent(
    Id<Basket> basketId, 
    Seller seller,
    decimal shippingAmountLeft) : BaseBasketDomainEvent(basketId.Value)
{
    public Seller Seller { get; } = seller;
    public decimal ShippingAmountLeft { get; } = shippingAmountLeft;
}