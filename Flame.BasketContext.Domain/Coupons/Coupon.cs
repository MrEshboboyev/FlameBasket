using Flame.BasketContext.Domain.Coupons.Events;
using Flame.Common.Domain.Services;

namespace Flame.BasketContext.Domain.Coupons;

public sealed class Coupon : AggregateRoot<Coupon>
{
    #region Constructors
    
    private Coupon(
        string code,
        Amount amount,
        DateRange validityPeriod)
    {
        Code = code.EnsureLengthInRange(6, 10);
        Amount = amount.EnsureNonNull();
        ValidityPeriod = validityPeriod.EnsureNonNull();
        IsActive = true;
    }
    
    #endregion
    
    #region Factory Methods
    
    public static Coupon Create(
        string code,
        Amount amount,
        DateRange dateRange)
    {
        var coupon = new Coupon(
            code, 
            amount,
            dateRange);
        
        #region Events
        
        coupon.RaiseDomainEvent(new CouponCreatedEvent(coupon.Id));

        #endregion
        
        return coupon;
    }
    
    #endregion

    #region Properties
    
    public string Code { get; }
    public bool IsActive { get; private set; }
    public Amount Amount { get; }
    public DateRange ValidityPeriod { get; }
    
    #endregion

    #region Own Methods
    
    public void Activate(IDateTimeProvider dateTimeProvider)
    {
        IsActive.EnsureFalse();
        ValidityPeriod.InRange(dateTimeProvider.UtcNow());
        IsActive = true;

        RaiseDomainEvent(new CouponActivatedEvent(Id));
    }

    public void Deactivate()
    {
        IsActive.EnsureTrue();
        IsActive = false;

        RaiseDomainEvent(new CouponDeactivatedEvent(Id));
    }
    
    #endregion
}