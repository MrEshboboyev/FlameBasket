namespace Flame.BasketContext.Application.Baskets.Dtos;

public class BasketDto
{
    public Guid Id { get; set; }
    public List<BasketItemGroupDto> BasketItems { get; set; } = [];
    public decimal TaxPercentage { get; set; }
    public decimal TotalAmount { get; set; }
    public CustomerDto Customer { get; set; }
    public Guid? CouponId { get; set; }
}