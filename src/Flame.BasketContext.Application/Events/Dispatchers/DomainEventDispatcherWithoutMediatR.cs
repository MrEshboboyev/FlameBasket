using Flame.Common.Domain.Events;
using Flame.Common.Domain.Primitives;
using Microsoft.Extensions.DependencyInjection;

namespace Flame.BasketContext.Application.Events.Dispatchers;

public class DomainEventDispatcherWithoutMediatr(
    IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        var eventQueue = new Queue<IDomainEvent>(events);

        while (eventQueue.Count > 0)
        {
            var domainEvent = eventQueue.Dequeue();
            var handlerType = typeof(IDomainEventHandler<>)
                .MakeGenericType(domainEvent.GetType());
            var handlers = serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var handleMethod = handlerType.GetMethod("Handle");
                if (handleMethod == null) continue;

                await (Task)handleMethod
                    .Invoke(
                        handler, 
                        [ domainEvent, cancellationToken ])!;

                // If the handler raises additional events, add them to the queue
                if (domainEvent is not IAggregateRoot aggregateRoot) continue;
                
                var additionalEvents = aggregateRoot.PopDomainEvents();
                foreach (var additionalEvent in additionalEvents)
                {
                    eventQueue.Enqueue(additionalEvent);
                }
            }
        }
    }
}