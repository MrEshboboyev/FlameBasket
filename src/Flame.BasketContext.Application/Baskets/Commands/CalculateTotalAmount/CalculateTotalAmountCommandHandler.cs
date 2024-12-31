using Flame.BasketContext.Application.Abstractions.Commands;
using Flame.BasketContext.Domain.Baskets;
using Flame.BasketContext.Domain.Baskets.Services;
using Flame.Common.Domain.Primitives;

namespace Flame.BasketContext.Application.Baskets.Commands.CalculateTotalAmount;

internal sealed class CalculateTotalAmountCommandHandler(
    IBasketRepository basketRepository,
    ICouponService couponService,
    IDomainEventDispatcher domainEventDispatcher,
    IUnitOfWork unitOfWork) 
    : CommandHandlerBase<CalculateTotalAmountCommand, decimal>(
        domainEventDispatcher, unitOfWork)
{
    private Basket? _basket;
    
    protected override async Task<Result<decimal, IDomainError>> ExecuteAsync(
        CalculateTotalAmountCommand request,
        CancellationToken cancellationToken)
    {
        var basketId = request.BasketId;
        
        #region Get Basket
        
        _basket = await basketRepository.GetByIdAsync(basketId);
        if (_basket == null)
        {
            return Result.Failure<decimal, IDomainError>(
                DomainError.NotFound());
        }
        
        #endregion
        
        #region Calculate Total Amount in Basket

        await _basket.CalculateTotalAmountAsync(couponService);

        #endregion
        
        return Result.Success<decimal, IDomainError>(_basket.TotalAmount);
    }

    protected override IAggregateRoot? GetAggregateRoot(
        Result<decimal, IDomainError> result)
    {
        return _basket;
    }
}