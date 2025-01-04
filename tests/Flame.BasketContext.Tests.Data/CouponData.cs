using Flame.BasketContext.Domain.Coupons;
using Flame.Common.Domain.ValueObjects;

namespace Flame.BasketContext.Tests.Data;

public static class CouponData
{
    public static string Code = "TL343443";
    public static Amount PercentageAmount = Amount.Percentage(34);
    public static Amount FixAmount = Amount.Fix(12);
    public static DateRange Range = DateRange.FromString("2024-01-01", "2024-08-31");
}