using Flame.BasketContext.Application.Abstractions.Commands;
using Flame.BasketContext.Domain.Baskets;
using Flame.Common.Domain.Primitives;
using MediatR;

namespace Flame.BasketContext.Application.Baskets.Commands.UpdateBasketItemCount;

public class UpdateBasketItemCountCommandHandler(
    IBasketRepository basketRepository,
    IDomainEventDispatcher domainEventDispatcher,
    IUnitOfWork unitOfWork) 
    : CommandHandlerBase<UpdateBasketItemCountCommand, Unit>(
        domainEventDispatcher, unitOfWork)
{
    private Basket? _basket;
    
    protected override async Task<Result<Unit, IDomainError>> ExecuteAsync(
        UpdateBasketItemCountCommand request, CancellationToken cancellationToken)
    {
        var (basketId, itemId, quantity) = request;
        
        #region Get Basket
        
        _basket = await basketRepository.GetByIdAsync(basketId);
        if (_basket == null)
        {
            return Result.Failure<Unit, IDomainError>(
                DomainError.NotFound());
        }
        
        #endregion
        
        #region Get this item from basket

        var item = _basket.BasketItems.SelectMany(x 
            => x.Value.Items).FirstOrDefault(i => i.Id.Value == itemId);
        if (item == null)
        {
            return Result.Failure<Unit, IDomainError>(
                DomainError.NotFound());
        }

        #endregion
        
        #region Update Item count in Basket

        _basket.UpdateItemCount(item, quantity);

        #endregion
        
        return Result.Success<Unit, IDomainError>(Unit.Value);
    }

    protected override IAggregateRoot? GetAggregateRoot(Result<Unit, IDomainError> result)
    {
        return _basket;
    }
}