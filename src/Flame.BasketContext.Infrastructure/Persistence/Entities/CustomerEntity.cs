namespace Flame.BasketContext.Infrastructure.Persistence.Entities;

public class CustomerEntity
{
    public Guid Id { get; set; }
    public bool IsEliteMember { get; set; }
    public ICollection<BasketEntity> Baskets { get; set; }
}