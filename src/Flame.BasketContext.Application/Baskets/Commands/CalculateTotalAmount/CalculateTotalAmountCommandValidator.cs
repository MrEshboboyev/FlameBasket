using FluentValidation;

namespace Flame.BasketContext.Application.Baskets.Commands.CalculateTotalAmount;

public class CalculateTotalAmountCommandValidator : AbstractValidator<CalculateTotalAmountCommand>
{
    public CalculateTotalAmountCommandValidator()
    {
        RuleFor(x => x.BasketId).NotEmpty();
    }
}