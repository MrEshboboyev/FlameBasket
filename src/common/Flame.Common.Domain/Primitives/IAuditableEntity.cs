namespace Flame.Common.Domain.Primitives;

public interface IAuditableEntity
{
    public DateTimeOffset CreatedAtUtc { get; }
    public DateTimeOffset LastModifiedAtUtc { get; }

}