using Flame.BasketContext.Application.Abstractions.Commands;
using MediatR;

namespace Flame.BasketContext.Application.Coupons.Commands.ApplyCoupon;

public record ApplyCouponCommand(
    Guid BasketId,
    Guid CouponId) : ICommand<Unit>;