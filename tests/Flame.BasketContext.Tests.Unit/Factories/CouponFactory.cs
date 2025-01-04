using Flame.BasketContext.Domain.Coupons;
using Flame.BasketContext.Tests.Data;
using Flame.Common.Domain.ValueObjects;
using static Flame.BasketContext.Tests.Data.CouponData;

namespace Flame.BasketContext.Tests.Unit.Factories;

public static class CouponFactory
{
    public static Coupon Create(
        string? code = null,
        Amount? amount = null,
        DateRange? dateRange = null)
    {
        return Coupon.Create(
            code ?? Code,
            amount ?? PercentageAmount, 
            dateRange ?? CouponData.Range);
    }
}