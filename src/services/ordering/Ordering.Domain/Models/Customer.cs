namespace Ordering.Domain.Models;

public class Customer : Entity<CustomerId>
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    // Protected parameterless constructor required by Entity Framework Core
    protected Customer() { }

    // Private constructor to prevent direct instantiation using the 'new' keyword
    private Customer(CustomerId id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    /// <summary>
    /// Used to create a new customer instance for the first time.
    /// </summary>
    public static Customer Create(CustomerId id, string name, string email)
    {
        // Validation rules using DomainException
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Customer name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email address cannot be null or empty.");

        var customer = new Customer(id, name, email);

        // Example of adding a domain event:
        // customer.AddDomainEvent(new CustomerCreatedEvent(customer));

        return customer;
    }

    /// <summary>
    /// Used to reconstitute a customer object, usually when retrieving it from the database.
    /// </summary>
    public static Customer Of(CustomerId id, string name, string email)
    {
        return new Customer(id, name, email);
    }
}