using Flame.BasketContext.Tests.Data;
using static Flame.BasketContext.Tests.Data.BasketData;

namespace Flame.BasketContext.Tests.Unit.Factories;

public static class BasketFactory
{
    public static Basket Create(
        decimal? taxPercentage = null,
        Customer? customer = null)
    {
        return Basket.Create(
            taxPercentage ?? TaxAmount,
            customer?? BasketData.Customer);
    }
}