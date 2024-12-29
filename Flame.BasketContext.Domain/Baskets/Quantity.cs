namespace Flame.BasketContext.Domain.Baskets;

/// <summary>
/// Represents a quantity value object.
/// </summary>
public sealed class Quantity : ValueObject
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Quantity"/> class with the specified parameters.
    /// </summary>
    /// <param name="value">The value of the quantity.</param>
    /// <param name="limit">The limit of the quantity.</param>
    /// <param name="pricePerUnit">The price per unit of the quantity.</param>
    private Quantity(int value, int limit, decimal pricePerUnit)
    {
        value.EnsureGreaterThan(0);
        limit.EnsureGreaterThan(0);
        limit.EnsureAtLeast(value);
        pricePerUnit.EnsureGreaterThan(0);
        Value = value;
        Limit = limit;
        PricePerUnit = pricePerUnit;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a new instance of the <see cref="Quantity"/> class.
    /// </summary>
    /// <param name="value">The value of the quantity.</param>
    /// <param name="limit">The limit of the quantity.</param>
    /// <param name="pricePerUnit">The price per unit of the quantity.</param>
    /// <returns>A new instance of the <see cref="Quantity"/> class.</returns>
    public static Quantity Create(int value, int limit, decimal pricePerUnit)
    {
        return new Quantity(value, limit, pricePerUnit);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the value of the quantity.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Gets the limit of the quantity.
    /// </summary>
    public int Limit { get; }

    /// <summary>
    /// Gets the price per unit of the quantity.
    /// </summary>
    public decimal PricePerUnit { get; }

    /// <summary>
    /// Gets the total price of the quantity.
    /// </summary>
    public decimal TotalPrice => Value * PricePerUnit;

    #endregion

    #region Methods

    /// <summary>
    /// Updates the value of the quantity.
    /// </summary>
    /// <param name="newValue">The new value of the quantity.</param>
    /// <returns>A new instance of the <see cref="Quantity"/> class with the updated value.</returns>
    public Quantity UpdateValue(int newValue)
    {
        return Create(newValue, Limit, PricePerUnit);
    }

    /// <summary>
    /// Updates the limit of the quantity.
    /// </summary>
    /// <param name="newLimit">The new limit of the quantity.</param>
    /// <returns>A new instance of the <see cref="Quantity"/> class with the updated limit.</returns>
    public Quantity UpdateLimit(int newLimit)
    {
        return Create(Value, newLimit, PricePerUnit);
    }

    /// <summary>
    /// Updates the price per unit of the quantity.
    /// </summary>
    /// <param name="newPricePerUnit">The new price per unit of the quantity.</param>
    /// <returns>A new instance of the <see cref="Quantity"/> class with the updated price per unit.</returns>
    public Quantity UpdatePrice(decimal newPricePerUnit)
    {
        return Create(Value, Limit, newPricePerUnit);
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Gets the equality components for the quantity.
    /// </summary>
    /// <returns>An enumerable of equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Limit;
        yield return PricePerUnit;
    }

    #endregion
}