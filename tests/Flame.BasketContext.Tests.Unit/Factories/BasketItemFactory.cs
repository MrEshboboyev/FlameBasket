using Flame.Common.Domain.Primitives;
using static Flame.BasketContext.Tests.Data.BasketItemData;


namespace Flame.BasketContext.Tests.Unit.Factories;

public static class BasketItemFactory
{
    public static BasketItem Create(
        string? name = null,
        Quantity? quantity = null, 
        string? imageUrl = null,
        Seller? seller = null, 
        Id<BasketItem>? id = null)
    {
        return BasketItem.Create(
            name?? BasketItemName, 
            quantity?? BasketItemQuantity, 
            imageUrl?? BasketItemImageUrl,
            seller?? SellerFactory.Create(),
            id?? BasketItemId);
    }
}