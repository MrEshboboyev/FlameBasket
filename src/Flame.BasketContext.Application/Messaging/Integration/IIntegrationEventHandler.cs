using Flame.Common.Core.Events;

namespace Flame.BasketContext.Application.Messaging.Integration;

public interface IIntegrationEventHandler<in TEvent> 
    where TEvent : IntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
