using Flame.Common.Domain.Events;
using Flame.Common.Domain.Primitives;

namespace Flame.BasketContext.Application.Abstractions;

public abstract class CommandHandlerBase<TCommand, TResponse>(
    IDomainEventDispatcher domainEventDispatcher,
    IUnitOfWork unitOfWork)
    : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
    public async Task<Result<TResponse, IDomainError>> Handle(
        TCommand request, 
        CancellationToken cancellationToken)
    {
        // Step 1. Execute core operation
        var operationResult = await ExecuteAsync(request, cancellationToken);
        if (!operationResult.IsSuccess)
        {
            return operationResult;
        }
        
        // Step 2. Commit Unit of Work
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Step 3. Dispatch Domain Events
        var aggregateRoot = GetAggregateRoot(operationResult);
        if (aggregateRoot == null) return operationResult;
        
        var domainEvents = aggregateRoot.PopDomainEvents();
        await DispatchDomainEventsAsync(domainEvents, cancellationToken);

        // Step 4. Return Result
        return operationResult;
    }
    
    #region Own Methods
    
    /// <summary>
    /// Executes the core operation logic for the command.
    /// </summary>
    protected abstract Task<Result<TResponse, IDomainError>> ExecuteAsync(
        TCommand request,
        CancellationToken cancellationToken);

    /// <summary>
    /// Extracts the aggregate root for dispatching domain events.
    /// </summary>
    protected abstract IAggregateRoot? GetAggregateRoot(
        Result<TResponse, IDomainError> result);

    /// <summary>
    /// Manually dispatches a collection of domain events.
    /// </summary>
    private async Task DispatchDomainEventsAsync(
        IEnumerable<IDomainEvent>? domainEvents,
        CancellationToken cancellationToken)
    {
        if (domainEvents == null) return;
        
        await domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
    }
    
    #endregion
}