using Flame.BasketContext.Tests.Data;
using Flame.Common.Domain.Exceptions;

namespace Flame.BasketContext.Tests.Unit;

public class BasketItemTests
{
    #region Create
    
    [Fact]
    public void Create_WhenValidArgumentProvided_ShouldCreateBasketItem()
    {
        //Arrange & Act
        var basketItem = BasketItem.Create(
            BasketItemData.BasketItemName,
            BasketItemData.BasketItemQuantity,
            BasketItemData.BasketItemImageUrl,
            BasketItemData.Seller);

        //Assert
        basketItem.Should().NotBeNull();
    }

    #endregion
    
    #region Update
    
    [Theory]
    [InlineData(10, 13)]
    [InlineData(8, 15)]
    [InlineData(3, 19)]
    [InlineData(49, 56)]
    [InlineData(101, 134)]
    public void UpdateCount_WhenItemCountIsGreaterThanLimit_ShouldFail(
        int basketItemLimit,
        int basketItemCount)
    {
        //Arrange
        var quantity = TestFactories.QuantityFactory.Create(limit: basketItemLimit);
        var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity);

        //Act
        var action = () => basketItem.UpdateCount(basketItemCount);

        //Assert
        action.Should().ThrowExactly<ValidationException>();
    }

    [Theory]
    [InlineData(20, 13)]
    [InlineData(80, 15)]
    [InlineData(31, 19)]
    [InlineData(491, 56)]
    [InlineData(181, 134)]
    public void UpdateCount_WhenItemCountIsLessThanLimit_ShouldUpdateBasketItem(
        int basketItemLimit, 
        int basketItemCount)
    {
        //Arrange
        var quantity = TestFactories.QuantityFactory.Create(limit: basketItemLimit);
        var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity);

        //Act
        basketItem.UpdateCount(basketItemCount);

        //Assert
        basketItem.Quantity.Value.Should().Be(basketItemCount);

    }
    
    #endregion
}