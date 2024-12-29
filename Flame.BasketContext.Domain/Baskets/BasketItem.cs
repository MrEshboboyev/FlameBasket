namespace Flame.BasketContext.Domain.Baskets;

/// <summary>
/// Represents an item in a basket.
/// </summary>
public sealed class BasketItem : Entity<BasketItem>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BasketItem"/> class with the specified parameters.
    /// </summary>
    /// <param name="id">The identifier of the basket item.</param>
    /// <param name="name">The name of the basket item.</param>
    /// <param name="quantity">The quantity of the basket item.</param>
    /// <param name="imageUrl">The image URL of the basket item.</param>
    /// <param name="seller">The seller associated with the basket item.</param>
    private BasketItem(
        Id<BasketItem>? id,
        string name,
        Quantity quantity,
        string imageUrl,
        Seller seller)
        : base(id ?? Id<BasketItem>.New())
    {
        Name = name.EnsureNonBlank();
        ImageUrl = imageUrl.EnsureImageUrl();
        Quantity = quantity;
        Seller = seller;
        IsActive = true;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a new instance of the <see cref="BasketItem"/> class.
    /// </summary>
    /// <param name="name">The name of the basket item.</param>
    /// <param name="quantity">The quantity of the basket item.</param>
    /// <param name="imageUrl">The image URL of the basket item.</param>
    /// <param name="seller">The seller associated with the basket item.</param>
    /// <param name="id">The optional identifier of the basket item.</param>
    /// <returns>A new instance of the <see cref="BasketItem"/> class.</returns>
    public static BasketItem Create(
        string name,
        Quantity quantity,
        string imageUrl,
        Seller seller,
        Id<BasketItem>? id = null)
    {
        return new BasketItem(
            id,
            name,
            quantity,
            imageUrl,
            seller);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the name of the basket item.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the image URL of the basket item.
    /// </summary>
    public string ImageUrl { get; }

    /// <summary>
    /// The minimum item count.
    /// </summary>
    private const int MinItemCount = 1;

    /// <summary>
    /// Gets the quantity of the basket item.
    /// </summary>
    public Quantity Quantity { get; private set; }

    /// <summary>
    /// Gets the seller associated with the basket item.
    /// </summary>
    public Seller Seller { get; }

    /// <summary>
    /// Gets a value indicating whether the basket item is active.
    /// </summary>
    public bool IsActive { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Updates the quantity of the basket item.
    /// </summary>
    /// <param name="basketItemCount">The new quantity.</param>
    public void UpdateCount(int basketItemCount)
    {
        basketItemCount.EnsureGreaterThan(MinItemCount);
        Quantity = Quantity.UpdateValue(basketItemCount);
    }

    #region Activate/Deactivate

    /// <summary>
    /// Deactivates the basket item.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Activates the basket item.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    #endregion

    #endregion
}