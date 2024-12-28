using Flame.Common.Domain.Events;
using Flame.Common.Domain.Extensions;
using Flame.Common.Domain.Primitives;

namespace Flame.Common.Domain.Entities;

/// <summary>
/// Represents an aggregate root entity that contains domain events.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public abstract class AggregateRoot<TModel>
    : Entity<TModel>, IAggregateRoot
    where TModel : IAuditableEntity
{
    #region Properties

    /// <summary>
    /// The list of domain events associated with this aggregate root.
    /// </summary>
    private readonly IList<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Gets the read-only collection of domain events.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    #endregion

    #region Methods

    /// <summary>
    /// Pops the domain events from this aggregate root.
    /// </summary>
    /// <returns>A read-only collection of domain events.</returns>
    public IReadOnlyCollection<IDomainEvent> PopDomainEvents()
    {
        var events = _domainEvents.ToList();
        ClearEvents();
        return events;
    }

    /// <summary>
    /// Clears the domain events associated with this aggregate root.
    /// </summary>
    public void ClearEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Raises a domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to raise.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        domainEvent.EnsureNonNull();
        _domainEvents.Add(domainEvent);
    }

    #endregion
}