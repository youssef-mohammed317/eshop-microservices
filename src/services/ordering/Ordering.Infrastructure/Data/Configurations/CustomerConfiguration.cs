using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        // Convert the Strongly-Typed ID to a standard Guid for the database
        builder.Property(c => c.Id)
               .HasConversion(
                   customerId => customerId.Value,// 1. When saving to DB: Take the Guid inside the CustomerId.
                   dbId => CustomerId.Of(dbId));// 2. When reading from DB: Take the Guid and wrap it in a new CustomerId.

        builder.Property(c => c.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Email)
               .HasMaxLength(255)
               .IsRequired();

        // Optional: Ensure email is unique
        builder.HasIndex(c => c.Email).IsUnique();
    }
}
