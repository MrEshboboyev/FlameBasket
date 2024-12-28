namespace Flame.BasketContext.Domain.Baskets;

public sealed class Seller : Entity<Seller>
{
    #region Constructors

    private Seller(
        Id<Seller> sellerId,
        string name,
        float rating,
        decimal shippingLimit,
        decimal shippingCost)
    {
        Name = name;    
        Rating = rating;
        ShippingLimit = shippingLimit;
        ShippingCost = shippingCost;
    }
    
    #endregion
    
    #region Factory Methods

    public static Seller Create(
        Id<Seller> sellerId,
        string name,
        float rating,
        decimal shippingLimit,
        decimal shippingCost)
    {
        return new Seller(
            sellerId ?? Id<Seller>.New(),
            name,
            rating,
            shippingLimit,
            shippingCost);
    }
    
    #endregion
    
    #region Properties

    public string Name { get; }
    public float Rating { get; }
    public decimal ShippingLimit { get; }
    public decimal ShippingCost { get; }
    
    #endregion
    
    #region Own methods

    public int GetLimitForProduct(string productName, ISellerLimitService limitService)
    {
        return limitService.GetLimitForProduct(Id, productName);
    }
    
    #endregion
}