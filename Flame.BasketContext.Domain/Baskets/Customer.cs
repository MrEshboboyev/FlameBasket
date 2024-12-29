namespace Flame.BasketContext.Domain.Baskets;

/// <summary>
/// Represents a customer entity.
/// </summary>
public class Customer : Entity<Customer>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Customer"/> class with the specified parameters.
    /// </summary>
    /// <param name="isEliteMember">Indicates whether the customer is an elite member.</param>
    /// <param name="id">The optional identifier of the customer.</param>
    private Customer(
        bool isEliteMember,
        Id<Customer>? id)
        : base(id ?? Id<Customer>.FromString("00000000-0000-0000-0000-000000000001"))
    {
        IsEliteMember = isEliteMember;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a new instance of the <see cref="Customer"/> class.
    /// </summary>
    /// <param name="isEliteMember">Indicates whether the customer is an elite member.</param>
    /// <param name="id">The optional identifier of the customer.</param>
    /// <returns>A new instance of the <see cref="Customer"/> class.</returns>
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

    /// <summary>
    /// Gets a value indicating whether the customer is an elite member.
    /// </summary>
    public bool IsEliteMember { get; }

    /// <summary>
    /// Gets the discount percentage for elite members.
    /// </summary>
    public decimal DiscountPercentage => IsEliteMember ? 0.1m : 0;

    #endregion
}