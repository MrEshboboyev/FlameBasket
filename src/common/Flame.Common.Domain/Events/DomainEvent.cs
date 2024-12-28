using System.Reflection;
using Flame.Common.Domain.Events.Decorators;

namespace Flame.Common.Domain.Events;

/// <summary>
/// Represents a domain event.
/// </summary>
public class DomainEvent : IDomainEvent
{
    #region Properties

    /// <summary>
    /// Gets or sets the version of the event.
    /// </summary>
    public int Version { get; set; } = 1;

    /// <summary>
    /// Gets the type of the event.
    /// </summary>
    public string EventType { get; set; }

    /// <summary>
    /// Gets the unique identifier of the event.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the date and time when the event occurred in UTC.
    /// </summary>
    public DateTimeOffset OccurredOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the trace information.
    /// </summary>
    public string? TraceInfo { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the aggregate associated with the event.
    /// </summary>
    public Guid AggregateId { get; set; }

    /// <summary>
    /// Gets the type of the aggregate associated with the event.
    /// </summary>
    public string AggregateType { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEvent"/> class with the specified aggregate ID and occurrence date.
    /// </summary>
    /// <param name="aggregateId">The unique identifier of the aggregate associated with the event.</param>
    /// <param name="occurredOnUtc">The date and time when the event occurred in UTC.</param>
    /// <exception cref="ArgumentNullException">Thrown when the aggregate ID is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the aggregate type is null.</exception>
    protected DomainEvent(Guid aggregateId, DateTimeOffset occurredOnUtc)
    {
        AggregateId = aggregateId != Guid.Empty
            ? aggregateId
            : throw new ArgumentNullException(nameof(aggregateId));
        OccurredOnUtc = occurredOnUtc;
        AggregateType = GetAggregateType(GetType())
                        ?? throw new InvalidOperationException("Aggregate type cannot be null.");
        EventType = GetEventType(this);
    }

    #endregion

    #region Get Aggregate Type

    /// <summary>
    /// Gets the aggregate type for the specified event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <returns>The aggregate type.</returns>
    public static string GetAggregateType<TEvent>() where TEvent : IDomainEvent =>
        GetAggregateType(typeof(TEvent));

    private static string GetAggregateType(Type eventType)
    {
        var attribute = eventType.GetCustomAttribute<AggregateTypeAttribute>();
        return attribute?.AggregateType ?? string.Empty;
    }

    #endregion

    #region Get Event Type

    /// <summary>
    /// Gets the event type for the specified event.
    /// </summary>
    /// <param name="event">The domain event.</param>
    /// <returns>The event type.</returns>
    private static string GetEventType(IDomainEvent @event) =>
        GetEventType(@event.GetType(), @event.AggregateType);

    /// <summary>
    /// Gets the event type for the specified event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <returns>The event type.</returns>
    public static string GetEventType<TEvent>() where TEvent : IDomainEvent =>
        GetEventType(typeof(TEvent));

    private static string GetEventType(Type eventType, string? prefix = null)
    {
        prefix ??= GetAggregateType(eventType);
        return $"{prefix}.{eventType.Name}";
    }

    #endregion
}