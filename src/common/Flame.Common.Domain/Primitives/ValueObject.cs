namespace Flame.Common.Domain.Primitives;

/// <summary>
/// Represents a base class for value objects.
/// </summary>
public abstract class ValueObject
{
    #region Methods

    /// <summary>
    /// Gets the equality components for the value object.
    /// </summary>
    /// <returns>An enumerable of equality components.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    #endregion

    #region Overrides

    /// <summary>
    /// Determines whether the specified object is equal to the current value object.
    /// </summary>
    /// <param name="obj">The object to compare with the current value object.</param>
    /// <returns>true if the specified object is equal to the current value object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;
        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current value object.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }

    #endregion
}