using Flame.BasketContext.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flame.BasketContext.Infrastructure.Persistence.Configurations;

public class SellerConfiguration: IEntityTypeConfiguration<SellerEntity>
{
    public void Configure(EntityTypeBuilder<SellerEntity> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Rating)
            .IsRequired();

        builder.Property(s => s.ShippingLimit)
            .IsRequired();

        builder.Property(s => s.ShippingCost)
            .IsRequired();
    }
}