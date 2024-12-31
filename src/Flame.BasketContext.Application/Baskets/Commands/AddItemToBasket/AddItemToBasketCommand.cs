using Flame.BasketContext.Application.Abstractions.Commands;

namespace Flame.BasketContext.Application.Baskets.Commands.AddItemToBasket;

public sealed record AddItemToBasketCommand(
    Guid BasketId,
    SellerRequest Seller,
    BasketItemRequest BasketItem,
    QuantityRequest Quantity) : ICommand<Guid>
{
}

public sealed record QuantityRequest(
    int Value,
    int QuantityLimit,
    decimal PricePerUnit);

public sealed record SellerRequest(
    Guid Id,
    string Name,
    float Rating,
    decimal ShippingLimit,
    decimal ShippingCost);

public sealed record BasketItemRequest(
    Guid ItemId,
    string Name,
    string ImageUrl,
    int Limit); 
