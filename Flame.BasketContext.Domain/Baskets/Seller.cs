namespace Flame.BasketContext.Domain.Baskets;

/// <summary>
/// Represents a seller entity.
/// </summary>
public sealed class Seller : Entity<Seller>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Seller"/> class with the specified parameters.
    /// </summary>
    /// <param name="sellerId">The identifier of the seller.</param>
    /// <param name="name">The name of the seller.</param>
    /// <param name="rating">The rating of the seller.</param>
    /// <param name="shippingLimit">The shipping limit for the seller.</param>
    /// <param name="shippingCost">The shipping cost for the seller.</param>
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

    /// <summary>
    /// Creates a new instance of the <see cref="Seller"/> class.
    /// </summary>
    /// <param name="sellerId">The identifier of the seller.</param>
    /// <param name="name">The name of the seller.</param>
    /// <param name="rating">The rating of the seller.</param>
    /// <param name="shippingLimit">The shipping limit for the seller.</param>
    /// <param name="shippingCost">The shipping cost for the seller.</param>
    /// <returns>A new instance of the <see cref="Seller"/> class.</returns>
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

    /// <summary>
    /// Gets the name of the seller.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the rating of the seller.
    /// </summary>
    public float Rating { get; }

    /// <summary>
    /// Gets the shipping limit for the seller.
    /// </summary>
    public decimal ShippingLimit { get; }

    /// <summary>
    /// Gets the shipping cost for the seller.
    /// </summary>
    public decimal ShippingCost { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Gets the shipping limit for a specific product.
    /// </summary>
    /// <param name="productName">The name of the product.</param>
    /// <param name="limitService">The service to get the shipping limit.</param>
    /// <returns>The shipping limit for the product.</returns>
    public int GetLimitForProduct(
        string productName, 
        ISellerLimitService limitService)
    {
        return limitService.GetLimitForProduct(Id, productName);
    }

    #endregion
}