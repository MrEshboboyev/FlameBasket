﻿using Flame.BasketContext.Domain.Baskets;

namespace Flame.BasketContext.Data;

public static class BasketData
{
    public const decimal TaxAmount = 18;
    public static Customer Customer = Customer.Create(false, null);
}