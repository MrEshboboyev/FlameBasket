using Flame.Common.Domain.Extensions;

namespace Flame.Common.Domain.Primitives;

/// <summary>
/// Represents a strongly-typed identifier.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public sealed record Id<TModel> : IId
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Id{TModel}"/> class with the specified GUID value.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    public Id(Guid value) => Value = value.EnsureNotDefault(nameof(value));

    /// <summary>
    /// Initializes a new instance of the <see cref="Id{TModel}"/> class with a new GUID value.
    /// </summary>
    public Id() : this(Guid.NewGuid())
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the GUID value of the identifier.
    /// </summary>
    public Guid Value { get; init; }

    #endregion

    #region Factory methods

    /// <summary>
    /// Creates a new instance of the <see cref="Id{TModel}"/> class with a new GUID value.
    /// </summary>
    /// <returns>A new instance of the <see cref="Id{TModel}"/> class.</returns>
    public static Id<TModel> New() => new(Guid.NewGuid());

    /// <summary>
    /// Creates a new instance of the <see cref="Id{TModel}"/> class from an existing identifier.
    /// </summary>
    /// <typeparam name="TNewModel">The type of the new model.</typeparam>
    /// <param name="id">The existing identifier.</param>
    /// <returns>A new instance of the <see cref="Id{TModel}"/> class.</returns>
    public static Id<TModel> FromId<TNewModel>(Id<TNewModel> id) => new(id.Value);

    /// <summary>
    /// Creates a new instance of the <see cref="Id{TModel}"/> class from a GUID value.
    /// </summary>
    /// <param name="id">The GUID value.</param>
    /// <returns>A new instance of the <see cref="Id{TModel}"/> class.</returns>
    public static Id<TModel> FromGuid(Guid id) => new(id);

    /// <summary>
    /// Creates a new instance of the <see cref="Id{TModel}"/> class from a string value.
    /// </summary>
    /// <param name="id">The string value.</param>
    /// <returns>A new instance of the <see cref="Id{TModel}"/> class.</returns>
    public static Id<TModel> FromString(string id) => new(Guid.Parse(id));

    #endregion

    #region Implicit conversions

    public static implicit operator Guid?(Id<TModel>? id) => id?.Value;

    public static implicit operator Guid(Id<TModel> id) => id.Value;

    public static implicit operator Id<TModel>(Guid id) => new(id);

    #endregion

    #region Comparisons

    public int CompareTo(object? obj)
    {
        return obj switch
        {
            IId otherId => CompareTo(otherId),
            Guid otherGuid => CompareTo(otherGuid),
            null => 1,
            _ => throw new ArgumentException("Object must be of type IId or Guid", nameof(obj))
        };
    }

    public int CompareTo(IId? other) => other?.Value.CompareTo(Value) ?? 1;

    public int CompareTo(Guid other) => Value.CompareTo(other);

    #endregion

    #region Equality

    public bool Equals(IId? other) => other?.Value == Value;

    #endregion
}