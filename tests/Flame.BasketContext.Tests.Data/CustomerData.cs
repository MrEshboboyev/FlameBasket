using Flame.BasketContext.Domain.Baskets;
using Flame.Common.Domain.Primitives;

namespace Flame.BasketContext.Tests.Data;

public static class CustomerData
{
    public static Customer EliteCustomer = Customer.Create(
        true,
        Id<Customer>.New());
    public static Customer NonEliteCustomer = Customer.Create(
        false,
        null);
}