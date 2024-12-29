using Flame.BasketContext.Domain.Coupons.Enums;

namespace Flame.BasketContext.Domain.Coupons;

public sealed class Amount : ValueObject
{
    #region Constructors
    
    private Amount(
        decimal value,
        CouponType couponType)
    {
        Value = value.EnsureGreaterThan(0);
        CouponType = couponType.EnsureNonNull();
    }
    
    #endregion

    #region Factory Methods
    
    public static Amount Fix(decimal value) => new(value, CouponType.Fix);
    public static Amount Percentage(decimal value) => new(value, CouponType.Percentage);

    #endregion
    
    #region Properties
    
    public decimal Value { get; }
    public CouponType CouponType { get; }
    
    #endregion
    
    #region Overrides
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return CouponType;
    }
    
    #endregion
}