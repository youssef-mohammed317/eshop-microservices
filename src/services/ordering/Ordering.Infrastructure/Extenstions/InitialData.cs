using System;
using System.Collections.Generic;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.SeedData;

public static class InitialData
{
    public static IEnumerable<Customer> Customers => new List<Customer>
    {
        Customer.Create(CustomerId.Of(Guid.Parse("58c49479-ec65-4de2-86e7-033c546291aa")), "John Doe", "john@example.com"),
        Customer.Create(CustomerId.Of(Guid.Parse("84f8841a-a039-4467-bc5e-881a74d49a0d")), "Jane Smith", "jane@example.com")
    };

    public static IEnumerable<Product> Products => new List<Product>
    {
        Product.Create(ProductId.Of(Guid.Parse("5334c996-8457-4cf0-815c-ed2b77c4ff61")), "IPhone 15 Pro", 999.00m),
        Product.Create(ProductId.Of(Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), "MacBook Pro", 1999.00m),
        Product.Create(ProductId.Of(Guid.Parse("c228807d-5a82-411a-ab4d-3b74db1ed538")), "AirPods Pro", 249.00m)
    };

    public static IEnumerable<Order> Orders => new List<Order>
    {
        CreateOrder1(),
        CreateOrder2()
    };

    private static Order CreateOrder1()
    {
        var customerId = CustomerId.Of(Guid.Parse("58c49479-ec65-4de2-86e7-033c546291aa"));
        var iphoneId = ProductId.Of(Guid.Parse("5334c996-8457-4cf0-815c-ed2b77c4ff61"));
        var airpodsId = ProductId.Of(Guid.Parse("c228807d-5a82-411a-ab4d-3b74db1ed538"));

        var address = Address.Of("John", "Doe", "john@example.com", "123 Main St", "USA", "NY", "10001");
        var payment = Payment.Of("John Doe", "1234567890123456", "12/25", "123", 1);

        var order = Order.Create(
            OrderId.Of(Guid.Parse("9634df4e-f2d4-42f2-ad7a-b94f1c1fde0c")),
            customerId,
            OrderName.Of("Order-1"),
            address,
            address,
            payment
        );

        order.Add(iphoneId, 2, 999.00m);
        order.Add(airpodsId, 1, 249.00m);

        return order;
    }

    private static Order CreateOrder2()
    {
        var customerId = CustomerId.Of(Guid.Parse("84f8841a-a039-4467-bc5e-881a74d49a0d"));
        var macbookId = ProductId.Of(Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"));
        var airpodsId = ProductId.Of(Guid.Parse("c228807d-5a82-411a-ab4d-3b74db1ed538"));

        var address = Address.Of("Jane", "Smith", "jane@example.com", "456 Market St", "USA", "CA", "94016");
        var payment = Payment.Of("Jane Smith", "9876543210987654", "10/26", "456", 1);

        var order = Order.Create(
            OrderId.Of(Guid.Parse("b234df4e-f2d4-42f2-ad7a-b94f1c1fde0d")),
            customerId,
            OrderName.Of("Order-2"),
            address,
            address,
            payment
        );

        order.Add(macbookId, 1, 1999.00m);
        order.Add(airpodsId, 2, 249.00m);

        return order;
    }
}