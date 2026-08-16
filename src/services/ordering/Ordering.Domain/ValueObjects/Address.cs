namespace Ordering.Domain.ValueObjects;

public record Address
{
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string EmailAddress { get; } = default!;
    public string AddressLine { get; } = default!;
    public string Country { get; } = default!;
    public string State { get; } = default!;
    public string ZipCode { get; } = default!;

    // Protected parameterless constructor required by Entity Framework Core
    protected Address() { }

    // Private constructor to force the use of the factory method
    private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
    }

    /// <summary>
    /// Factory method to create a new Address instance with validation.
    /// </summary>
    public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        // Validation rules using DomainException
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new DomainException("Email address cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(addressLine))
            throw new DomainException("Address line cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(country))
            throw new DomainException("Country cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(state))
            throw new DomainException("State cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(zipCode))
            throw new DomainException("Zip code cannot be null or empty.");

        return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
    }
}