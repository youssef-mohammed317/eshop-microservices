using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;

namespace Ordering.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    // Constructor accepting DbContextOptions to configure the database provider and connection string
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for the 4 main entities
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // This line automatically finds and applies all configuration classes 
        // (classes that implement IEntityTypeConfiguration<T>) in the current assembly.
        // You will need these configurations to map your Strongly-Typed IDs and Value Objects (like Address, Payment).
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}