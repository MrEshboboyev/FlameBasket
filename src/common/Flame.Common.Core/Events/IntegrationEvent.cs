namespace Flame.Common.Core.Events;

/// <summary>
/// Represents the base class for integration events.
/// </summary>
public abstract class IntegrationEvent : IIntegrationEvent
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationEvent"/> class with the specified aggregate ID.
    /// </summary>
    /// <param name="aggregateId">The unique identifier of the aggregate associated with the event.</param>
    /// <exception cref="ArgumentNullException">Thrown when the aggregate ID is empty.</exception>
    protected IntegrationEvent(Guid aggregateId)
    {
        if (aggregateId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(aggregateId), 
                "Aggregate id cannot be empty");
        }

        AggregateId = aggregateId;
        EventType = GetType().Name;
    }

    #endregion

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
    /// Gets the date and time when the event occurred in UTC.
    /// </summary>
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets or sets the unique identifier of the aggregate associated with the event.
    /// </summary>
    public Guid AggregateId { get; set; }
}