using FluentValidation;

namespace Flame.BasketContext.Application.Baskets.Commands.UpdateBasketItemCount;

public class UpdateBasketItemCountCommandValidator : AbstractValidator<UpdateBasketItemCountCommand>
{
    public UpdateBasketItemCountCommandValidator()
    {
        RuleFor(x => x.BasketId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.Quantity).NotEmpty();
    }
}