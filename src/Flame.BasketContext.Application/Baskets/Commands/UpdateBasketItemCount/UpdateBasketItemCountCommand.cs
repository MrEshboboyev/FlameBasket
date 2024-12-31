using Flame.BasketContext.Application.Abstractions.Commands;
using MediatR;

namespace Flame.BasketContext.Application.Baskets.Commands.UpdateBasketItemCount;

public record UpdateBasketItemCountCommand(
    Guid BasketId,
    Guid ItemId,
    int Quantity) : ICommand<Unit>;