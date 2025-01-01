using System.Reflection;
using Flame.BasketContext.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flame.BasketContext.Infrastructure.Persistence;

public class BasketAppDbContext(DbContextOptions<BasketAppDbContext> options) : DbContext(options)
{
    public DbSet<BasketEntity> Baskets { get; set; }
    public DbSet<BasketItemEntity> BasketItems { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<SellerEntity> Sellers { get; set; }
    public DbSet<CouponEntity> Coupons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply Fluent API configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Optional: Add additional configurations if required

        base.OnModelCreating(modelBuilder);
    }

}