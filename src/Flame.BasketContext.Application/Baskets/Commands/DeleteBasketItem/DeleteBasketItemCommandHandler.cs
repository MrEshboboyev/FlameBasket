using Flame.BasketContext.Application.Abstractions.Commands;
using Flame.BasketContext.Domain.Baskets;
using Flame.Common.Domain.Primitives;
using MediatR;

namespace Flame.BasketContext.Application.Baskets.Commands.DeleteBasketItem;

public class DeleteBasketItemCommandHandler(
    IBasketRepository basketRepository,
    IDomainEventDispatcher domainEventDispatcher, 
    IUnitOfWork unitOfWork) 
    : CommandHandlerBase<DeleteBasketItemCommand, Unit>(
        domainEventDispatcher, unitOfWork)
{
    private Basket? _basket;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    protected override async Task<Result<Unit, IDomainError>> ExecuteAsync(
        DeleteBasketItemCommand request,
        CancellationToken cancellationToken)
    {
        var (basketId, itemId) = request;

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

        #region Delete this BasketItem

        _basket.DeleteItem(item);

        #endregion

        // Save changes to repository
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success<Unit, IDomainError>(Unit.Value);
    }

    protected override IAggregateRoot? GetAggregateRoot(Result<Unit, IDomainError> result)
    {
        return _basket;
    }
}