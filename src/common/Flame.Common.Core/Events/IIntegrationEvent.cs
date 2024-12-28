namespace Flame.Common.Core.Events;

/// <summary>
/// Represents a contract for integration events.
/// </summary>
public interface IIntegrationEvent
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
    /// Gets the unique identifier of the aggregate associated with the event.
    /// </summary>
    Guid AggregateId { get; }
}