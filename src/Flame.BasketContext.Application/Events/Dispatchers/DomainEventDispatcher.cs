using Flame.Common.Domain.Events;
using Flame.Common.Domain.Primitives;
using MediatR;

namespace Flame.BasketContext.Application.Events.Dispatchers;

// default Dispatcher
public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> initialEvents,
        CancellationToken cancellationToken = default)
    {
        var eventQueue = new Queue<IDomainEvent>(initialEvents);

        while (eventQueue.Count > 0)
        {
            var currentEvent = eventQueue.Dequeue();

            // Publish the current event
            await mediator.Publish(currentEvent, cancellationToken);

            // If the current event is associated with an aggregate, check for new events
            if (currentEvent is not IAggregateRoot aggregateRoot) continue;    
            
            var additionalEvents = aggregateRoot.PopDomainEvents();
            
            foreach (var additionalEvent in additionalEvents)
            {
                eventQueue.Enqueue(additionalEvent);
            }
        }
    }
}