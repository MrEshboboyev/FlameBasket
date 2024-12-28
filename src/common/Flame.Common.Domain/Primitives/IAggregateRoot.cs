using Flame.Common.Domain.Events;

namespace Flame.Common.Domain.Primitives;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    
    IReadOnlyCollection<IDomainEvent> PopDomainEvents();
    
    void ClearEvents();
}
