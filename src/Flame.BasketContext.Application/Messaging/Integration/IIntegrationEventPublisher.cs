using Flame.Common.Core.Events;

namespace Flame.BasketContext.Application.Messaging.Integration;

public interface IIntegrationEventPublisher
{
    Task PublishAsync<T>(T @event) where T : IntegrationEvent;
}
