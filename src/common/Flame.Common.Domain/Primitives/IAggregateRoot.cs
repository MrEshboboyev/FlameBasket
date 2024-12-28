using Flame.Common.Domain.Events;

namespace Flame.Common.Domain.Primitives;

/// <summary>
/// Represents an aggregate root with domain events.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Gets the domain events associated with this aggregate root.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Pops the domain events from this aggregate root.
    /// </summary>
    /// <returns>A read-only collection of domain events.</returns>
    IReadOnlyCollection<IDomainEvent> PopDomainEvents();

    /// <summary>
    /// Clears the domain events associated with this aggregate root.
    /// </summary>
    void ClearEvents();
}