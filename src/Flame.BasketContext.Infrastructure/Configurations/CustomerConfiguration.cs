using Flame.BasketContext.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flame.BasketContext.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.IsEliteMember)
            .IsRequired();
    }
}
