using Flame.BasketContext.Application.Abstractions.Commands;
using MediatR;

namespace Flame.BasketContext.Application.Baskets.Commands.DeleteBasketItem;

public record DeleteBasketItemCommand(
    Guid BasketId,
    Guid ItemId) : ICommand<Unit>;