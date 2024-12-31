using FluentValidation;

namespace Flame.BasketContext.Application.Coupons.Commands.ApplyCoupon;

public class ApplyCouponCommandValidator : AbstractValidator<ApplyCouponCommand>
{
    public ApplyCouponCommandValidator()
    {
        RuleFor(x => x.BasketId).NotEmpty();
        RuleFor(x => x.CouponId).NotEmpty();
    }
}