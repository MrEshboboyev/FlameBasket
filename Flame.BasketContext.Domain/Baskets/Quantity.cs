namespace Flame.BasketContext.Domain.Baskets;

public sealed class Quantity : ValueObject
{
    #region Constructors
    
    private Quantity(
        int value,
        int limit,
        decimal pricePerUnit)
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
    
    public static Quantity Create(
        int value, 
        int limit,
        decimal pricePerUnit)
    {
        return new Quantity(
            value,
            limit,
            pricePerUnit);
    }
    
    #endregion
    
    #region Properties
    
    public int Value { get; }
    public int Limit { get; }
    public decimal PricePerUnit { get; }
    public decimal TotalPrice => Value * PricePerUnit;
    
    #endregion
    
    #region Methods
    
    public Quantity UpdateValue(int newValue)
    {
        return Create(
            newValue,
            Limit,
            PricePerUnit);
    }

    public Quantity UpdateLimit(int newLimit)
    {
        return Create(
            Value,
            newLimit,
            PricePerUnit);
    }

    public Quantity UpdatePrice(decimal newPricePerUnit)
    {
        return Create(
            Value,
            Limit,
            newPricePerUnit);
    }
    
    #endregion
    
    #region Overrides
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Limit;
        yield return PricePerUnit;
    }
    
    #endregion
}