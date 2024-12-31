using Flame.BasketContext.Application.Abstractions.Commands;

namespace Flame.BasketContext.Application.Baskets.Commands.CalculateTotalAmount;

public record CalculateTotalAmountCommand(
    Guid BasketId) : ICommand<decimal>;