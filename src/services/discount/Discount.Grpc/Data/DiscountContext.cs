using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext(DbContextOptions<DiscountContext> options) : DbContext(options)
{
    public DbSet<Coupon> Coupons { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Coupon>().HasData(
            new Coupon
            {
                Id = 1,
                ProductName = "Smartphone",
                Description = "Launch Promo - $150 off",
                Amount = 150
            },
            new Coupon
            {
                Id = 2,
                ProductName = "Developer Laptop",
                Description = "Tech Upgrade - $200 off",
                Amount = 200
            },
            new Coupon
            {
                Id = 3,
                ProductName = "Silicone Case",
                Description = "Accessory Bundle Discount",
                Amount = 5
            }
        );
    }
}