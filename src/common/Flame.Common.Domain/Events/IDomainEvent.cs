using MediatR;

namespace Flame.Common.Domain.Events;

/// <summary>
/// Defines the contract for a domain event.
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Gets the version of the event.
    /// </summary>
    int Version { get; }

    /// <summary>
    /// Gets the type of the event.
    /// </summary>
    string EventType { get; }

    /// <summary>
    /// Gets the unique identifier of the event.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the date and time when the event occurred in UTC.
    /// </summary>
    DateTimeOffset OccurredOnUtc { get; }

    /// <summary>
    /// Gets the trace information.
    /// </summary>
    string? TraceInfo { get; }

    /// <summary>
    /// Gets the unique identifier of the aggregate associated with the event.
    /// </summary>
    Guid AggregateId { get; }

    /// <summary>
    /// Gets the type of the aggregate associated with the event.
    /// </summary>
    string AggregateType { get; }
}