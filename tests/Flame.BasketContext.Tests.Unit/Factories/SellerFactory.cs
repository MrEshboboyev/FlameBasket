using Flame.BasketContext.Tests.Data;
using Flame.Common.Domain.Primitives;

namespace Flame.BasketContext.Tests.Unit.Factories;

public static class SellerFactory
{
    public static Seller Create(
        Id<Seller>? sellerId = null,
        string? name = null,
        float? rating = null,
        decimal? shippingLimit = null, 
        decimal? shippingCost = null)
    {
        return Seller.Create(
            sellerId ?? BasketItemData.Seller.Id,
            name ?? BasketItemData.Seller.Name,
            rating ?? BasketItemData.Seller.Rating,
            shippingLimit ?? BasketItemData.Seller.ShippingLimit, 
            shippingCost ?? BasketItemData.Seller.ShippingCost);
    }
}