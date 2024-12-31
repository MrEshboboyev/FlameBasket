using Flame.BasketContext.Application.Abstractions.Queries;

namespace Flame.BasketContext.Application.Baskets.Queries.GetBasket;

public record GetBasketQuery(
    Guid BasketId) : IQuery<BasketDto>;