using Marten;
using Marten.Schema;
using Catalog.API.Models;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation))
        {
            return;
        }

        session.Store<Product>(GetPreconfiguredProducts());

        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
    {
        new Product
        {
            Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ef61"),
            Name = "Redragon K552 Mechanical Keyboard",
            Description = "Tenkeyless mechanical gaming keyboard with customizable backlighting.",
            ImageFile = "redragon-k552.png",
            Category = new List<string> { "Electronics", "Computer Peripherals", "Gaming" },
            Price = 45.99M
        },
        new Product
        {
            Id = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
            Name = "Samsung Galaxy A56",
            Description = "Android smartphone with advanced hardware features and seamless connectivity.",
            ImageFile = "samsung-a56.png",
            Category = new List<string> { "Electronics", "Smartphones", "Mobile Devices" },
            Price = 350.00M
        },
        new Product
        {
            Id = new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
            Name = "Dell Precision 3541 Laptop",
            Description = "High-performance workstation laptop designed for intensive development workflows.",
            ImageFile = "dell-precision-3541.png",
            Category = new List<string> { "Electronics", "Computers", "Laptops" },
            Price = 1200.00M
        }
    };
}