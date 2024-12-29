using Flame.Common.Domain.Events;

namespace Flame.BasketContext.Application.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default);
}