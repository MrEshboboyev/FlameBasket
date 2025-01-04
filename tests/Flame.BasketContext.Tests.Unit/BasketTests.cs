﻿using Flame.BasketContext.Domain.Baskets.Services;
using Flame.BasketContext.Domain.Coupons;
using Flame.BasketContext.Tests.Data;
using Flame.BasketContext.Tests.Unit.Extensions;
using Flame.Common.Domain.Exceptions;
using NSubstitute;

namespace Flame.BasketContext.Tests.Unit;

public class BasketTests
{
    #region Own
    
    #region Create Basket 
    
    [Theory]
    [InlineData(.18)]
    [InlineData(.24)]
    [InlineData(.45)]
    public void Create_WhenValidArgumentProvided_ShouldCreateBasket(decimal taxPercentage)
    {
        //Arrange
        var customer = TestFactories.CustomerFactory.Create();

        //Act
        var basket = Basket.Create(taxPercentage, customer);

        //Assert
        basket.Should().NotBeNull();
    }
    
    #endregion
    
    #region Update Basket
    
    [Fact]
    public void UpdateItemCount_WhenBasketItemDoesNotExist_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        // Act
        var action = () => basket.UpdateItemCount(basketItem, 5);

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    [Fact]
    public void UpdateItemCount_WhenBasketItemExists_ShouldRaiseBasketItemCountUpdatedEvent()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        var expectedEvent = new BasketItemCountUpdatedEvent(basket.Id, basketItem, 5);

        basket.AddItem(basketItem);

        // Act
        basket.UpdateItemCount(basketItem, 5);

        // Assert
        basket.DomainEvents.Should().HaveCount(2);
        basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
    }

    #endregion
    
    #region Delete Basket
    
    [Fact]
    public void DeleteItem_WhenBasketItemDoesNotExist_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        // Act
        var action = () => basket.DeleteItem(basketItem);

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    [Fact]
    public void DeleteItem_WhenBasketItemExists_ShouldRaiseBasketItemDeletedEvent()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        var expectedEvent = new BasketItemDeletedEvent(basket.Id, basketItem);

        basket.AddItem(basketItem);

        // Act
        basket.DeleteItem(basketItem);

        // Assert
        basket.DomainEvents.Should().HaveCount(2);
        basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
    }
    
    #endregion

    #region Calculate shipping amount
    
    [Theory]
    [InlineData(300, 45, new object[] { 100.0, 100.0 })]
    [InlineData(500, 120, new object[] { 100.0, 100.0 })]
    [InlineData(120, 34, new object[] { 100.0, 100.0 })]
    [InlineData(185, 30, new object[] { 100.0, 100.0 })]
    public void CalculateShippingAmount_WhenBasketItemIsUnderLimit_ShouldRaiseShippingAmountCalculatedEvent(
        decimal shippingLimit, 
        decimal shippingCost, 
        object[] basketItemPrices)
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var seller = TestFactories.SellerFactory.Create(
            shippingLimit: shippingLimit,
            shippingCost: shippingCost);

        var decimalList = basketItemPrices.OfType<double>()
            .Select(x => (decimal)x)
            .ToList();


        // Add basket items to the basket
        decimalList.ForEach(price =>
        {
            var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
            var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
            basket.AddItem(basketItem);
        });

        // Calculate total item price
        var totalItemPrice = decimalList.Sum();

        // Determine shipping amount
        var shippingAmount = totalItemPrice > shippingLimit
            ? 0 
            : shippingLimit - totalItemPrice;

        // Create the expected event
        var expectedEvent = new ShippingAmountCalculatedEvent(
            basket.Id,
            seller,
            shippingAmount);

        // Act
        basket.CalculateShippingAmount(seller);

        // Assert
        basket.DomainEvents.Should().HaveCount(3);
        basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
    }

    [Fact]
    public void CalculateShippingAmount_WhenSellerArgumentIsNull_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();

        // Act
        var action = () => basket.CalculateShippingAmount(null!);

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    [Fact]
    public void CalculateShippingAmount_WhenSellerIsValidBasketIsEmpty_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var seller = TestFactories.SellerFactory.Create();

        // Act
        var action = () => basket.CalculateShippingAmount(seller);

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    [Fact]
    public void CalculateShippingAmount_WhenSellersBasketItemsIsNull_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var seller = TestFactories.SellerFactory.Create();
        basket.BasketItems.Add(
            seller,
            (null, 0m)!);

        // Act
        var action = () => basket.CalculateShippingAmount(seller);

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }
    
    #endregion

    #region Calculate Items count
    
    [Fact]
    public void
        CalculateBasketItemsAmount_WhenBasketItemsIsEmpty_ShouldRaiseBasketItemsAmountCalculatedEventWithZeroAmount()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var expectedEvent = new BasketItemsAmountCalculatedEvent(basket.Id, 0);

        // Act
        basket.CalculateBasketItemsAmount();

        // Assert
        basket.DomainEvents.Should().HaveCount(1);
        basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
    }

    [Fact]
    public void
        CalculateBasketItemsAmount_WhenBasketItemsIsEmpty_ShouldRaiseBasketItemsAmountCalculatedEventWithAmount()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();

        var basketItem = TestFactories.BasketItemFactory.Create();
        basket.AddItem(basketItem);

        var expectedEvent = new BasketItemsAmountCalculatedEvent(
            basket.Id,
            basketItem.Quantity.TotalPrice);

        // Act
        basket.CalculateBasketItemsAmount();

        // Assert
        basket.DomainEvents.Should().HaveCount(2);
        basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
    }

    #endregion
    
    #region Calculate Total Amount

    [Theory]
    [InlineData(200, 50, new object[] { 100.0, 400.0 })]
    [InlineData(500, 60, new object[] { 400, 150 })]
    public async Task CalculateTotalAmount_WhenBasketIsNotEmpty_ShouldCalculateTotalAmount(
        int shippingAmount, decimal shippingCost, object[] basketItemPrices)
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var seller = TestFactories.SellerFactory.Create(
            shippingLimit: shippingAmount,
            shippingCost: shippingCost);

        var decimalList = basketItemPrices.OfType<double>()
            .Select(x => (decimal)x)
            .ToList();

        // Add basket items to the basket
        decimalList.ForEach(price =>
        {
            var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
            var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
            basket.AddItem(basketItem);
        });

        var total = decimalList.Sum();
        var expectedTotalAmount = total + (total * 18 / 100);

        // Act
        await basket.CalculateTotalAmountAsync(null!);

        // Assert
        basket.TotalAmount.Should().Be(expectedTotalAmount);
    }

    [Theory]
    [InlineData(200, 50, new object[] { 100.0, 400.0 })]
    [InlineData(500, 60, new object[] { 400, 150 })]
    public async Task CalculateTotalAmount_WhenBasketCustomerIsEliteMember_ShouldCalculateTotalAmountWithEliteDiscount(
        int shippingAmount,
        decimal shippingCost,
        object[] basketItemPrices)
    {
        // Arrange
        var eliteCustomer = CustomerData.EliteCustomer;
        var basket = TestFactories.BasketFactory.Create(customer: eliteCustomer);
        var seller = TestFactories.SellerFactory.Create(
            shippingLimit: shippingAmount,
            shippingCost: shippingCost);

        var decimalList = basketItemPrices.OfType<double>()
            .Select(x => (decimal)x)
            .ToList();

        // Add basket items to the basket
        decimalList.ForEach(price =>
        {
            var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
            var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
            basket.AddItem(basketItem);
        });

        var total = decimalList.Sum();
        total = total - (total * eliteCustomer.DiscountPercentage);
        var expectedTotalAmount = total + (total * 18 / 100);

        // Act
        await basket.CalculateTotalAmountAsync(null!);

        // Assert
        basket.TotalAmount.Should().Be(expectedTotalAmount);
    }
    
    #endregion
    
    #region Assign Customer

    [Fact]
    public void AssignCustomer_WhenCustomerIsLogged_ShouldRaiseCustomerAssignedEvent()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var customer = TestFactories.CustomerFactory.Create();

        // Act
        basket.AssignCustomer(customer);

        // Assert
        basket.Customer.Should().Be(customer);
    }
    
    #endregion
    
    #endregion
    
    #region Items
    
    #region Add Item to Basket
    
    [Fact]
    public void AddItem_WhenBasketItemIsAdded_ShouldRaiseBasketItemAddedEvent()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        var expectedEvent = new BasketItemAddedEvent(basket.Id, basketItem);

        // Act
        basket.AddItem(basketItem);

        // Assert
        var actualEvent = basket.DomainEvents.Single();
        actualEvent.Should().BeEquivalentEventTo(expectedEvent);
    }
    
    #endregion
    
    #region Deactivate Item

    [Fact]
    public void DeactivateBasketItem_WhenBasketItemExists_ShouldRaiseBasketItemDeactivatedEvent()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        var expectedEvent = new BasketItemDeactivatedEvent(basket.Id, basketItem);

        basket.AddItem(basketItem);

        // Act
        basket.DeactivateBasketItem(basketItem);

        // Assert
        basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
    }

    [Fact]
    public void DeactivateBasketItem_WhenBasketItemDoesNotExist_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        // Act
        var action = () => basket.DeactivateBasketItem(basketItem);

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    [Fact]
    public void DeactivateBasketItem_WhenBasketItemIsAlreadyDeactivated_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        basket.AddItem(basketItem);
        basket.DeactivateBasketItem(basketItem);

        // Act
        var action = () => basket.DeactivateBasketItem(basketItem);

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    #endregion
    
    #region Activate Item
    
    [Fact]
    public void ActivateBasketItem_WhenBasketItemExists_ShouldRaiseBasketItemActivatedEvent()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        var expectedEvent = new BasketItemActivatedEvent(basket.Id, basketItem);

        basket.AddItem(basketItem);
        basket.DeactivateBasketItem(basketItem);

        // Act
        basket.ActivateBasketItem(basketItem);

        // Assert
        basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
    }

    [Fact]
    public void ActivateBasketItem_WhenBasketItemDoesNotExist_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        // Act
        var action = () => basket.ActivateBasketItem(basketItem);

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    [Fact]
    public void ActivateBasketItem_WhenBasketItemIsAlreadyActivated_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var basketItem = TestFactories.BasketItemFactory.Create();

        basket.AddItem(basketItem);

        // Act
        var action = () => basket.ActivateBasketItem(basketItem);

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    #endregion
    
    #endregion
    
    #region Coupon
    
    #region Calculate Total Amount with Coupon
    
    [Theory]
    [InlineData(200, 50, new object[] { 100.0, 400.0 }, 90)]
    [InlineData(500, 60, new object[] { 400, 150 }, 50)]
    public async Task CalculateTotalAmount_WhenHasActiveCouponWithFixValue_ShouldCalculateTotalAmountWithCoupon(
        int shippingAmount, 
        decimal shippingCost, 
        object[] basketItemPrices, 
        decimal fixCouponAmount)
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var coupon = TestFactories.CouponFactory.Create(amount: Amount.Fix(fixCouponAmount));

        var couponService = Substitute.For<ICouponService>();

        couponService.IsActive(coupon.Id).Returns(Task.FromResult(true));

        await basket.ApplyCouponAsync(
            coupon.Id,
            couponService);

        var seller = TestFactories.SellerFactory.Create(
            shippingLimit: shippingAmount,
            shippingCost: shippingCost);

        var decimalList = basketItemPrices.OfType<double>()
            .Select(x => (decimal)x)
            .ToList();

        // Add basket items to the basket
        decimalList.ForEach(price =>
        {
            var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
            var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
            basket.AddItem(basketItem);
        });

        var total = decimalList.Sum();


        couponService.ApplyDiscountAsync(coupon.Id, total)
            .Returns(Task.FromResult(total - coupon.Amount.Value));

        total = await couponService.ApplyDiscountAsync(coupon.Id, total);

        var expectedTotalAmount = total + (total * 18 / 100);


        // Act
        await basket.CalculateTotalAmountAsync(couponService);

        // Assert
        basket.TotalAmount.Should().Be(expectedTotalAmount);
    }


    [Theory]
    [InlineData(200, 50, new object[] { 100.0, 400.0 }, .9)]
    [InlineData(500, 60, new object[] { 400, 150 }, .13)]
    public async Task CalculateTotalAmount_WhenHasActiveCouponWithPercentage_ShouldCalculateTotalAmountWithCoupon(
        int shippingAmount, decimal shippingCost, object[] basketItemPrices, decimal percentageCouponAmount)
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var coupon = TestFactories.CouponFactory.Create(amount: Amount.Percentage(percentageCouponAmount));

        var couponService = NSubstitute.Substitute.For<ICouponService>();
        couponService.IsActive(coupon.Id).Returns(Task.FromResult(true));

        await basket.ApplyCouponAsync(
            coupon.Id,
            couponService);

        var seller = TestFactories.SellerFactory.Create(shippingLimit: shippingAmount, shippingCost: shippingCost);

        var decimalList = basketItemPrices.OfType<double>()
            .Select(x => (decimal)x)
            .ToList();

        // Add basket items to the basket
        decimalList.ForEach(price =>
        {
            var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
            var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
            basket.AddItem(basketItem);
        });

        var total = decimalList.Sum();

        couponService.ApplyDiscountAsync(coupon.Id, total)
            .Returns(Task.FromResult(total - (total * coupon.Amount.Value)));

        total = await couponService.ApplyDiscountAsync(coupon.Id, total);

        var expectedTotalAmount = total + (total * 18 / 100);

        // Act
        await basket.CalculateTotalAmountAsync(couponService);

        // Assert
        basket.TotalAmount.Should().Be(expectedTotalAmount);
    }

    #endregion
    
    #region Apply Coupon
    
    [Fact]
    public async Task ApplyCoupon_WhenCouponIsNotActive_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var coupon = TestFactories.CouponFactory.Create();
        coupon.Deactivate();

        var couponService = NSubstitute.Substitute.For<ICouponService>();

        couponService.IsActive(coupon.Id).Returns(Task.FromResult(coupon.IsActive));

        // Act
        var action = async () => await basket.ApplyCouponAsync(coupon.Id, couponService);

        // Assert
        await action.Should().ThrowExactlyAsync<ValidationException>();
    }

    #endregion
    
    #region Remove Coupon
    
    [Fact]
    public void RemoveCoupon_WhenCouponIdIsNull_ShouldFail()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();

        // Act
        var action = () => basket.RemoveCoupon();

        // Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    [Fact]
    public async Task RemoveCoupon_WhenCouponIdIsNotNull_ShouldRemoveCoupon()
    {
        // Arrange
        var basket = TestFactories.BasketFactory.Create();
        var coupon = TestFactories.CouponFactory.Create();

        var couponService = Substitute.For<ICouponService>();

        couponService.IsActive(coupon.Id).Returns(Task.FromResult(coupon.IsActive));

        await basket.ApplyCouponAsync(coupon.Id, couponService);

        // Act
        basket.RemoveCoupon();

        // Assert
        basket.CouponId.Should().BeNull();
    }
    
    #endregion
    
    #endregion
}