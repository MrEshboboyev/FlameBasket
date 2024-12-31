using Flame.BasketContext.Application.Abstractions.Commands;
using Flame.BasketContext.Domain.Baskets;
using Flame.Common.Domain.Primitives;

namespace Flame.BasketContext.Application.Baskets.Commands.AddItemToBasket;

public class AddItemToBasketCommandHandler(
    IBasketRepository basketRepository,
    IUnitOfWork unitOfWork,
    IDomainEventDispatcher domainEventDispatcher)
    : CommandHandlerBase<AddItemToBasketCommand, Guid>(domainEventDispatcher, unitOfWork)
{
    private Basket? _basket;

    protected override async Task<Result<Guid, IDomainError>> ExecuteAsync(
        AddItemToBasketCommand request,
        CancellationToken cancellationToken)
    {
        var (basketId, seller, basketItem, quantity) = request;
        
        #region Create Objests (Quantity, Seller)
        
        var quantityObj = Quantity.Create(
            quantity.Value,
            quantity.QuantityLimit, 
            quantity.PricePerUnit);

        var sellerObj = Seller.Create(
            seller.Id,
            seller.Name,
            seller.Rating,
            seller.ShippingLimit,
            seller.ShippingCost);

        #endregion
        
        #region Get Basket
        
        _basket = await basketRepository.GetByIdAsync(basketId);
        if (_basket == null)
        {
            return Result.Failure<Guid, IDomainError>(DomainError.NotFound());
        }
        
        #endregion

        #region Create BasketItem
        
        var basketItemObj = BasketItem.Create(
            basketItem.Name,
            quantityObj,
            basketItem.ImageUrl,
            sellerObj,
            basketItem.ItemId);

        #endregion
        
        #region Add Item to Basket
        
        _basket.AddItem(basketItemObj);
        
        #endregion

        #region Add Database
        
        await basketRepository.AddBasketItemAsync(basketId, basketItemObj);
        
        #endregion

        return Result.Success<Guid, IDomainError>(basketItemObj.Id.Value);
    }

    protected override IAggregateRoot? GetAggregateRoot(Result<Guid, IDomainError> result)
    {
        // Return the created aggregate root to dispatch domain events
        return _basket;
    }
}