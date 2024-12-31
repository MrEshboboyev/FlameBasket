using Flame.BasketContext.Application.Abstractions.Queries;

namespace Flame.BasketContext.Application.Baskets.Queries.GetBasket;

public class GetBasketQueryHandler(
    IBasketRepository basketRepository, 
    IMapper mapper)
    : IQueryHandler<GetBasketQuery, BasketDto>
{
    public async Task<Result<BasketDto, IDomainError>> Handle(
        GetBasketQuery request, 
        CancellationToken cancellationToken)
    {
        var basketId = request.BasketId;
        
        #region Get this basket
        
        var basket = await basketRepository.GetByIdAsync(basketId);
        if (basket is null)
            return Result.Failure<BasketDto, IDomainError>(
                DomainError.NotFound("Basket not found."));
        
        #endregion

        #region Mapping basket to dto
        
        // Map the Basket domain object to a BasketDto
        var basketDto = mapper.Map<BasketDto>(basket);
        
        #endregion
        
        return Result.Success<BasketDto, IDomainError>(basketDto);
    }
}