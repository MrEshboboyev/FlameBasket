namespace Flame.Common.Domain.Events.Decorators;

/// <summary>
/// Specifies the aggregate type for a domain event.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AggregateTypeAttribute(string aggregateType) : Attribute
{
    public string AggregateType { get; } = aggregateType;
}