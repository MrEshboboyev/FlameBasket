using Flame.BasketContext.Domain.Baskets;
using Flame.Common.Core.Events;
using Flame.Common.Domain.Primitives;

namespace Flame.BasketContext.Application.Events.Integration;

public sealed class BasketCreatedIntegrationEvent(
    Id<Basket> basketId,
    Guid customerId)
    : IntegrationEvent(basketId.Value)
{
    public Guid CustomerId { get; set; } = customerId;
}