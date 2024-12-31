using Flame.BasketContext.Application.Abstractions.Commands;
using Flame.BasketContext.Domain.Baskets.Services;
using Flame.BasketContext.Domain.Coupons;
using Flame.Common.Domain.Primitives;
using MediatR;

namespace Flame.BasketContext.Application.Coupons.Commands.ApplyCoupon;

public class ApplyCouponCommandHandler(
    ICouponRepository couponRepository,
    IBasketRepository basketRepository,
    ICouponService couponService,
    IDomainEventDispatcher domainEventDispatcher,
    IUnitOfWork unitOfWork) 
    : CommandHandlerBase<ApplyCouponCommand, Unit>(
        domainEventDispatcher, unitOfWork)
{
    private Coupon? _coupon;
    
    protected override async Task<Result<Unit, IDomainError>> ExecuteAsync(
        ApplyCouponCommand request,
        CancellationToken cancellationToken)
    {
        var (basketId, couponId) = request;
        
        #region Get this coupon
        
        var coupon = await couponRepository.GetByIdAsync(couponId);
        if (coupon is null)
        {
            return Result.Failure<Unit, IDomainError>(
                DomainError.NotFound($"Coupon with id {couponId} was not found."));
        }
        
        #endregion
        
        #region Get this Basket
        
        var basket = await basketRepository.GetByIdAsync(basketId);
        if (basket is null)
        {
            return Result.Failure<Unit, IDomainError>(
                DomainError.NotFound($"Basket with id {basketId} was not found."));
        }
        
        #endregion
        
        #region Apply coupon for this basket
        
        await basket.ApplyCouponAsync(coupon.Id, couponService);
        
        #endregion
        
        return Result.Success<Unit, IDomainError>(Unit.Value);
    }

    protected override IAggregateRoot? GetAggregateRoot(Result<Unit, IDomainError> result)
    {
        return _coupon;
    }
}