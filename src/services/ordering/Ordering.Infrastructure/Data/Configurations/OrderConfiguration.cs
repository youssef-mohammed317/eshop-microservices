using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
               .HasConversion(
                   orderId => orderId.Value,// 1. When saving to DB: Take the Guid inside the OrderId.
                   dbId => OrderId.Of(dbId));// 2. When reading from DB: Take the Guid and wrap it in a new OrderId.

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();

        // Map Relationship: Order has many OrderItems
        builder.HasMany(o => o.OrderItems)
               .WithOne()
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade); // Deleting an order deletes its items

        // Map OrderName Value Object to a simple string column
        builder.Property(o => o.OrderName)
               .HasConversion(
                   orderName => orderName.Value,
                   dbString => OrderName.Of(dbString))
               .HasMaxLength(100)
               .IsRequired();

        // Enum conversion (Stores the enum as a string in the DB instead of an int for better readability)
        builder.Property(o => o.OrderStatus)
               .HasConversion(
                   status => status.ToString(),
                   dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus))
               .HasMaxLength(50);

        // Map ShippingAddress Value Object (EF Core 8 approach)
        builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.EmailAddress).HasMaxLength(100);
            addressBuilder.Property(a => a.AddressLine).HasMaxLength(150).IsRequired();
            addressBuilder.Property(a => a.Country).HasMaxLength(50);
            addressBuilder.Property(a => a.State).HasMaxLength(50);
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(15).IsRequired();
        });

        // Map BillingAddress Value Object
        builder.ComplexProperty(o => o.BillingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.EmailAddress).HasMaxLength(100);
            addressBuilder.Property(a => a.AddressLine).HasMaxLength(150).IsRequired();
            addressBuilder.Property(a => a.Country).HasMaxLength(50);
            addressBuilder.Property(a => a.State).HasMaxLength(50);
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(15).IsRequired();
        });

        // Map Payment Value Object
        builder.ComplexProperty(o => o.Payment, paymentBuilder =>
        {
            paymentBuilder.Property(p => p.CardName).HasMaxLength(50);
            paymentBuilder.Property(p => p.CardNumber).HasMaxLength(25).IsRequired();
            paymentBuilder.Property(p => p.Expiration).HasMaxLength(10);
            paymentBuilder.Property(p => p.Cvv).HasMaxLength(4);
            paymentBuilder.Property(p => p.PaymentMethod);
        });


        builder.Ignore(o => o.TotalPrice);

    }
}