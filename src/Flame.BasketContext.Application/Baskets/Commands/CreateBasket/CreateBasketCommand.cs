using Flame.BasketContext.Application.Abstractions.Commands;

namespace Flame.BasketContext.Application.Baskets.Commands.CreateBasket;

public sealed record CreateBasketCommand(
    CustomerDto Customer,
    decimal TaxPercentage) : ICommand<Guid>;