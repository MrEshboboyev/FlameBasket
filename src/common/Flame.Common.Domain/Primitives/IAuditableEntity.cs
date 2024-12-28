namespace Flame.Common.Domain.Primitives;

/// <summary>
/// Represents an entity that is auditable.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Gets the date and time when the entity was created in UTC.
    /// </summary>
    public DateTimeOffset CreatedAtUtc { get; }

    /// <summary>
    /// Gets the date and time when the entity was last modified in UTC.
    /// </summary>
    public DateTimeOffset LastModifiedAtUtc { get; }
}