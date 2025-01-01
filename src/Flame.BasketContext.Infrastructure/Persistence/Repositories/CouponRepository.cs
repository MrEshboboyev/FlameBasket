using AutoMapper;
using Flame.BasketContext.Application.Repositories;
using Flame.BasketContext.Domain.Coupons;
using Flame.BasketContext.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flame.BasketContext.Infrastructure.Persistence.Repositories;

public class CouponRepository(
    BasketAppDbContext dbContext,
    IMapper mapper) : ICouponRepository
{
    public async Task<Coupon?> GetByIdAsync(Guid id)
    {
        var couponEntity = await dbContext.Coupons
            .Include(c => c.Baskets)
            .ThenInclude(b => b.BasketItems)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        return mapper.Map<Coupon>(couponEntity);
    }

    public async Task<bool> IsExistsAsync(Guid id)
    {
        return await dbContext.Coupons.AnyAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Coupon>> GetAllAsync()
    {
        var couponEntities = await dbContext.Coupons
            .Include(c => c.Baskets)
            .ThenInclude(b => b.BasketItems)
            .ToListAsync();
        
        return mapper.Map<IEnumerable<Coupon>>(couponEntities);
    }

    public async Task AddAsync(
        Coupon entity,
        CancellationToken cancellationToken)
    {
        var couponEntity = mapper.Map<CouponEntity>(entity);
        await dbContext.Coupons.AddAsync(couponEntity, cancellationToken);
    }

    public Task UpdateAsync(Coupon entity)
    {
        return Task.FromResult(dbContext.Update(entity));
    }

    public Task DeleteAsync(Guid id)
    {
        return Task.FromResult(dbContext.Remove(id));
    }
}