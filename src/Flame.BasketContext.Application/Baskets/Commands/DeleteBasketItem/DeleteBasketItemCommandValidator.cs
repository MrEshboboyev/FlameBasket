using FluentValidation;

namespace Flame.BasketContext.Application.Baskets.Commands.DeleteBasketItem;

public class DeleteBasketItemCommandValidator : AbstractValidator<DeleteBasketItemCommand>
{
    public DeleteBasketItemCommandValidator()
    {
        RuleFor(x => x.BasketId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
    }
}