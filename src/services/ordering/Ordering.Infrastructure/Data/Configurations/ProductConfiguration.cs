using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .HasConversion(
                   productId => productId.Value,
                   dbId => ProductId.Of(dbId));

        builder.Property(p => p.Name)
               .HasMaxLength(100)
               .IsRequired();

        // Always specify precision for decimal properties to avoid EF Core warnings
        builder.Property(p => p.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();
    }
}
