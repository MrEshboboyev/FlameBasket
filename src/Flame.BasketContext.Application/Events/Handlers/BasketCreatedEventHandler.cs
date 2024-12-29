using Flame.BasketContext.Application.Events.Integration;
using Flame.BasketContext.Application.Messaging.Integration;
using Flame.BasketContext.Domain.Baskets.Events;
using Flame.Common.Domain.Events;

namespace Flame.BasketContext.Application.Events.Handlers;

public class BasketCreatedEventHandler(IIntegrationEventPublisher integrationEventPublisher)
    : IDomainEventHandler<BasketCreatedEvent>
{
    public async Task Handle(
        BasketCreatedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new BasketCreatedIntegrationEvent(
            domainEvent.AggregateId, 
            domainEvent.CustomerId);
        
        await integrationEventPublisher.PublishAsync(integrationEvent);
    }
}