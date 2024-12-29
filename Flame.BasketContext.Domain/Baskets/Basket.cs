using Flame.BasketContext.Domain.Baskets.Events;
using Flame.BasketContext.Domain.Baskets.Services;
using Flame.Common.Domain.Exceptions;

namespace Flame.BasketContext.Domain.Baskets;

/// <summary>
/// Represents a shopping basket.
/// </summary>
public sealed class Basket : AggregateRoot<Basket>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Basket"/> class with the specified tax percentage and customer.
    /// </summary>
    /// <param name="taxPercentage">The tax percentage to apply to the basket.</param>
    /// <param name="customer">The customer associated with the basket.</param>
    private Basket(
        decimal taxPercentage, 
        Customer customer)
    {
        BasketItems = new Dictionary<Seller, (IList<BasketItem>, decimal)>();
        TaxPercentage = taxPercentage.EnsurePositive();
        TotalAmount = 0;
        Customer = customer;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a new instance of the <see cref="Basket"/> class with the specified tax percentage and customer.
    /// </summary>
    /// <param name="taxPercentage">The tax percentage to apply to the basket.</param>
    /// <param name="customer">The customer associated with the basket.</param>
    /// <returns>A new instance of the <see cref="Basket"/> class.</returns>
    public static Basket Create(decimal taxPercentage, Customer customer)
    {
        var basket = new Basket(taxPercentage, customer);
        // basket.RaiseDomainEvent(new BasketCreatedEvent(basket.Id, customer.Id));
        return basket;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the basket items grouped by seller.
    /// </summary>
    public IDictionary<Seller, 
        (IList<BasketItem> Items, decimal ShippingAmountLeft)> BasketItems { get; private set; }

    /// <summary>
    /// Gets the tax percentage to apply to the basket.
    /// </summary>
    public decimal TaxPercentage { get; }

    /// <summary>
    /// Gets the total amount of the basket.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets the customer associated with the basket.
    /// </summary>
    public Customer Customer { get; private set; }

    /// <summary>
    /// Gets the identifier of the applied coupon, if any.
    /// </summary>
    public Id<Coupon>? CouponId { get; private set; } = null;

    #endregion

    #region Methods

    /// <summary>
    /// Calculates the total amount of the basket.
    /// </summary>
    /// <returns>The total amount of the basket.</returns>
    private decimal CalculateTotalBasketAmount()
    {
        return (from seller in BasketItems.Keys
            let totalAmountBySeller = CalculateSellerAmount(seller)
            let costOfShipping = CalculateShippingCost(seller, totalAmountBySeller)
            select costOfShipping + totalAmountBySeller).Sum();
    }

    /// <summary>
    /// Applies the tax to the specified amount.
    /// </summary>
    /// <param name="amount">The amount to which the tax is applied.</param>
    /// <returns>The amount with the tax applied.</returns>
    private decimal ApplyTax(decimal amount)
    {
        return amount + ((amount * TaxPercentage) / 100);
    }

    /// <summary>
    /// Calculates the total amount of the basket, applying discounts and tax.
    /// </summary>
    /// <param name="couponService">The coupon service to apply discounts.</param>
    public async Task CalculateTotalAmount(ICouponService couponService)
    {
        var totalAmount = CalculateTotalBasketAmount();
        totalAmount = await ApplyCouponDiscount(totalAmount, couponService);
        totalAmount = ApplyEliteMemberDiscount(totalAmount);
        TotalAmount = ApplyTax(totalAmount);

        RaiseDomainEvent(new TotalAmountCalculatedEvent(this.Id, totalAmount));
    }

    #endregion

    #region BasketItem Methods

    /// <summary>
    /// Calculates the total amount of the basket items.
    /// </summary>
    public void CalculateBasketItemsAmount()
    {
        decimal totalBasketItemsAmount = 0;

        if (BasketItems.Count > 0)
        {
            totalBasketItemsAmount += BasketItems
                .Keys
                .Sum(CalculateSellerAmount);
        }

        RaiseDomainEvent(new BasketItemsAmountCalculatedEvent(Id, totalBasketItemsAmount));
    }

    #region Add/Update/Delete

    /// <summary>
    /// Adds an item to the basket.
    /// </summary>
    /// <param name="basketItem">The item to add to the basket.</param>
    public void AddItem(BasketItem basketItem)
    {
        if (BasketItems.TryGetValue(basketItem.Seller, out var value))
        {
            value.Items.Add(basketItem);
        }
        else
        {
            BasketItems.Add(
                basketItem.Seller,
                (new List<BasketItem>
                {
                    basketItem
                }, basketItem.Seller.ShippingLimit));
        }

        RaiseDomainEvent(new BasketItemAddedEvent(
            Id,
            basketItem));
    }

    /// <summary>
    /// Updates the quantity of an item in the basket.
    /// </summary>
    /// <param name="basketItem">The item to update.</param>
    /// <param name="count">The new quantity.</param>
    public void UpdateItemCount(BasketItem basketItem, int count)
    {
        BasketItems.EnsureKeyExists(basketItem.Seller);

        var existingBasketItem = BasketItems[basketItem.Seller]
            .Items
            .FirstOrDefault(x => x.Id == basketItem.Id);

        existingBasketItem.EnsureNonNull();

        existingBasketItem!.UpdateCount(count);

        RaiseDomainEvent(new BasketItemCountUpdatedEvent(
            Id,
            basketItem, 
            count));
    }

    /// <summary>
    /// Deletes an item from the basket.
    /// </summary>
    /// <param name="basketItem">The item to delete.</param>
    public void DeleteItem(BasketItem basketItem)
    {
        BasketItems.EnsureKeyExists(basketItem.Seller);

        var items = BasketItems[basketItem.Seller].Items;
        items.EnsureNonNull();

        items.Remove(basketItem);

        RaiseDomainEvent(new BasketItemDeletedEvent(
            Id,
            basketItem!));
    }

    /// <summary>
    /// Deletes all items from the basket.
    /// </summary>
    public void DeleteAll()
    {
        BasketItems.Clear();
        RaiseDomainEvent(new BasketItemsDeletedEvent(Id));
    }

    #endregion

    #region Activate/Deactivate

    /// <summary>
    /// Deactivates an item in the basket.
    /// </summary>
    /// <param name="basketItem">The item to deactivate.</param>
    public void DeactivateBasketItem(BasketItem basketItem)
    {
        basketItem.IsActive.EnsureTrue();

        BasketItems.EnsureKeyExists(basketItem.Seller);

        var items = BasketItems[basketItem.Seller].Items;
        items.EnsureNonNull();

        var existingBasketItem = items.FirstOrDefault(x => x.Id == basketItem.Id);
        existingBasketItem.EnsureNonNull();

        existingBasketItem!.Deactivate();

        RaiseDomainEvent(new BasketItemDeactivatedEvent(
            Id, 
            basketItem));
    }

    /// <summary>
    /// Activates an item in the basket.
    /// </summary>
    /// <param name="basketItem">The item to activate.</param>
    public void ActivateBasketItem(BasketItem basketItem)
    {
        basketItem.IsActive.EnsureFalse();

        BasketItems.EnsureKeyExists(basketItem.Seller);

        var items = BasketItems[basketItem.Seller].Items;
        items.EnsureNonNull();

        var existingBasketItem = items.FirstOrDefault(x => x.Id == basketItem.Id);
        existingBasketItem.EnsureNonNull();

        existingBasketItem!.Activate();

        RaiseDomainEvent(new BasketItemActivatedEvent(
            Id,
            basketItem));
    }

    #endregion

    #endregion

    #region Seller Methods

    /// <summary>
    /// Calculates the shipping amount for a seller.
    /// </summary>
    /// <param name="seller">The seller whose shipping amount is to be calculated.</param>
    public void CalculateShippingAmount(Seller seller)
    {
        var totalAmount = CalculateSellerAmount(seller);

        var valueTuple = BasketItems[seller];

        valueTuple.ShippingAmountLeft = totalAmount > seller.ShippingLimit
            ? 0 // No shipping cost
            : valueTuple.ShippingAmountLeft - totalAmount;

        RaiseDomainEvent(new ShippingAmountCalculatedEvent(
            Id,
            seller,
            valueTuple.ShippingAmountLeft));
    }

    /// <summary>
    /// Calculates the shipping cost for a seller.
    /// </summary>
    /// <param name="seller">The seller.</param>
    /// <param name="totalAmountBySeller">The total amount by seller.</param>
    /// <returns>The shipping cost.</returns>
    private static decimal CalculateShippingCost(
        Seller seller,
        decimal totalAmountBySeller)
    {
        return totalAmountBySeller > seller.ShippingLimit 
            ? 0 
            : seller.ShippingCost;
    }

    /// <summary>
    /// Calculates the total amount by seller.
    /// </summary>
    /// <param name="seller">The seller.</param>
    /// <returns>The total amount by seller.</returns>
    private decimal CalculateSellerAmount(Seller seller)
    {
        var (basketItems, _) = BasketItems.EnsureKeyExists(seller);

        var items = basketItems.EnsureNonNull();

        var totalAmount = items
            .Where(x => x.IsActive)
            .Sum(basketItem => basketItem.Quantity.TotalPrice);

        return totalAmount;
    }

    #endregion

    #region Customer Methods

    /// <summary>
    /// Assigns a customer to the basket.
    /// </summary>
    /// <param name="customer">The customer to assign.</param>
    public void AssignCustomer(Customer customer)
    {
        Customer = customer;
        RaiseDomainEvent(new CustomerAssignedEvent(
            Id,
            customer));
    }

    /// <summary>
    /// Applies an elite member discount to the total amount.
    /// </summary>
    /// <param name="totalAmount">The total amount before discount.</param>
    /// <returns>The total amount after applying the elite member discount.</returns>
    private decimal ApplyEliteMemberDiscount(decimal totalAmount)
    {
        return Customer.IsEliteMember
            ? totalAmount - (totalAmount * Customer.DiscountPercentage)
            : totalAmount;
    }

    #endregion

    #region Coupon Methods

    /// <summary>
    /// Applies a coupon to the basket.
    /// </summary>
    /// <param name="couponId">The identifier of the coupon to apply.</param>
    /// <param name="couponService">The coupon service to validate and apply the coupon.</param>
    public async Task ApplyCoupon(
        Id<Coupon> couponId,
        ICouponService couponService)
    {
        if (CouponId == couponId)
            return; // Already applied, no action needed.

        if (!await couponService.isActive(couponId))
        {
            throw new ValidationException("Coupon is not active!");
        }

        CouponId = couponId;
        RaiseDomainEvent(new CouponAppliedEvent(
            Id,
            couponId));
    }

    /// <summary>
    /// Removes the applied coupon from the basket.
    /// </summary>
    public void RemoveCoupon()
    {
        CouponId.EnsureNonNull();
        var couponId = CouponId;
        CouponId = null;
        RaiseDomainEvent(new CouponRemovedEvent(
            Id,
            couponId!));
    }

    /// <summary>
    /// Applies a coupon discount to the total amount.
    /// </summary>
    /// <param name="totalAmount">The total amount before discount.</param>
    /// <param name="couponService">The coupon service to apply the discount.</param>
    /// <returns>The total amount after applying the coupon discount.</returns>
    private async Task<decimal> ApplyCouponDiscount(
        decimal totalAmount,
        ICouponService couponService)
    {
        if (CouponId is not null)
        {
            return await couponService.ApplyDiscountAsync(
                CouponId,
                totalAmount);
        }

        return totalAmount;
    }

    #endregion
}