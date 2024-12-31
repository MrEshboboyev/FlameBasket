using Flame.BasketContext.Application.Abstractions.Commands;
using Flame.BasketContext.Domain.Baskets;
using Flame.Common.Domain.Primitives;

namespace Flame.BasketContext.Application.Baskets.Commands.CreateBasket;

internal sealed class CreateBasketCommandHandler(
    IBasketRepository basketRepository,
    IUnitOfWork unitOfWork,
    IDomainEventDispatcher domainEventDispatcher)
    : CommandHandlerBase<CreateBasketCommand, Guid>(domainEventDispatcher, unitOfWork)
{
    private Basket? _createdBasket;

    protected override async Task<Result<Guid, IDomainError>> ExecuteAsync(
        CreateBasketCommand request, 
        CancellationToken cancellationToken)
    {
        var (customer, taxPercentage) = request;
        
        #region Checking basket already exists for this customer
        
        // Step 1: Core operation
        if(await basketRepository.IsExistByCustomerIdAsync(request.Customer.Id))
        {
            return Result.Failure<Guid, IDomainError>(
                DomainError.Conflict("Basket already exist for the given customer"));
        }
        
        #endregion
        
        #region Create Objects (Customer)
        
        var customerObj = Customer.Create(
            customer.IsEliteMember, 
            customer.Id);
        
        #endregion
        
        #region Create Basket with percentage
        
        _createdBasket = Basket.Create(taxPercentage, customerObj);
        
        #endregion

        #region Add database
        
        await basketRepository.AddAsync(_createdBasket, cancellationToken);
        
        #endregion

        return Result.Success<Guid, IDomainError>(_createdBasket.Id);
    }

    protected override IAggregateRoot? GetAggregateRoot(Result<Guid, IDomainError> result)
    {
        // Return the created aggregate root to dispatch domain events
        return _createdBasket;
    }
}