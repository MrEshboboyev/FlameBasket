namespace Flame.BasketContext.Domain.Baskets;

public sealed class BasketItem : Entity<BasketItem>
{
    #region Constructors
    
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

    public static BasketItem Create(string name,
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

    public string Name { get; }
    public string ImageUrl { get; }
    public const int MinItemCount = 1;
    public Quantity Quantity { get; private set; }
    public Seller Seller { get; }
    public bool IsActive { get; private set; }
    
    #endregion
    
    #region Own Methods
    
    public void UpdateCount(int basketItemCount)
    {
        basketItemCount.EnsureGreaterThan(MinItemCount);
        Quantity = Quantity.UpdateValue(basketItemCount);
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }
    
    #endregion
}