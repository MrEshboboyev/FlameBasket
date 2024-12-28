using Flame.Common.Domain.Primitives;

namespace Flame.Common.Domain.Entities;

/// <summary>
/// Represents a base entity that is auditable.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public abstract class Entity<TModel> : IAuditableEntity
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TModel}"/> class with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    protected Entity(Id<TModel> id)
    {
        Id = id;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        LastModifiedAtUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TModel}"/> class with a new identifier.
    /// </summary>
    protected Entity() : this(Id<TModel>.New())
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the identifier of the entity.
    /// </summary>
    public Id<TModel> Id { get; }

    /// <summary>
    /// Gets the date and time when the entity was created in UTC.
    /// </summary>
    public DateTimeOffset CreatedAtUtc { get; }

    /// <summary>
    /// Gets the date and time when the entity was last modified in UTC.
    /// </summary>
    public DateTimeOffset LastModifiedAtUtc { get; }

    #endregion

    #region Overrides

    /// <summary>
    /// Determines whether the specified object is equal to the current entity.
    /// </summary>
    /// <param name="obj">The object to compare with the current entity.</param>
    /// <returns>true if the specified object is equal to the current entity; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Entity<TModel> entity)
        {
            return entity.Id == Id;
        }

        return false;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current entity.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    #endregion
}