using Flame.BasketContext.Domain.Baskets.Services;
using Flame.BasketContext.Infrastructure.Persistence;

namespace Flame.BasketContext.Infrastructure.Services;

public class SellerLimitService(BasketAppDbContext dbContext) : ISellerLimitService
{
    public int GetLimitForProduct(Guid sellerId, string productName)
    {
        // Retrieve seller limits based on sellerId and productName
        var seller = dbContext.Sellers.FirstOrDefault(s => s.Id == sellerId);

        if (seller == null)
            throw new InvalidOperationException("Seller not found.");

        // Placeholder for logic to determine product limits (if limits depend on a specific property in SellerEntity)
        return productName switch
        {
            "ProductA" => 10, // Example limit for ProductA
            "ProductB" => 20, // Example limit for ProductB
            "ProductC" => 15, // Example limit for ProductC
            _ => 0 // Default limit if no specific product match
        };
    }
}