using Flame.Common.Domain.Extensions;

namespace Flame.Common.Domain.Primitives;

public sealed record Id<TModel> : IId
{
    #region Constructors
    
    public Id(Guid value) => Value = value.EnsureNotDefault(nameof(value));
    
    public Id() : this(Guid.NewGuid()) { }
    
    #endregion
    
    #region Properties
    
    public Guid Value { get; init; }
    
    #endregion

    #region Factory methods
    
    public static Id<TModel> New() => new(Guid.NewGuid());
    public static Id<TModel> FromId<TNewModel>(Id<TNewModel> id) => new(id.Value);
    public static Id<TModel> FromGuid(Guid id) => new(id);
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