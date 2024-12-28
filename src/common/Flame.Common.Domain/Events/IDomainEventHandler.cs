using MediatR;

namespace Flame.Common.Domain.Events;

/// <summary>
/// Defines the contract for handling domain events.
/// </summary>
/// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}