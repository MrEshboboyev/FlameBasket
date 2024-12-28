namespace Flame.Common.Domain.Primitives;

/// <summary>
/// Represents a generic identifier.
/// </summary>
public interface IId :
    IComparable,
    IComparable<IId>,
    IComparable<Guid>,
    IEquatable<IId>
{
    /// <summary>
    /// Gets the GUID value of the identifier.
    /// </summary>
    Guid Value { get; }
}