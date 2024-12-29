using Ardalis.SmartEnum;

namespace Flame.BasketContext.Domain.Coupons.Enums;

public sealed class CouponType : SmartEnum<CouponType>
{
    public static readonly CouponType Fix = new CouponType(nameof(Fix), 1);
    public static readonly CouponType Percentage = new CouponType(nameof(Percentage), 2);

    private CouponType(string name, int value) : base(name, value) { }
}