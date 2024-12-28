namespace Flame.BasketContext.Domain.Baskets;

public class Customer : Entity<Customer>
{
    #region Constructors
    
    private Customer(
        bool isEliteMember,
        Id<Customer>? id) 
        : base(id ?? Id<Customer>.FromString("00000000-0000-0000-0000-000000000001"))
    {
        IsEliteMember = isEliteMember;
    }
    
    #endregion
    
    #region Factory Methods
    
    public static Customer Create(
        bool isEliteMember,
        Id<Customer>? id = null)
    {
        return new Customer(
            isEliteMember, 
            id);
    }
    
    #endregion
    
    #region Properties
    
    public bool IsEliteMember { get; }
    
    public decimal DiscountPercentage => IsEliteMember ? 0.1m : 0;

    #endregion
}