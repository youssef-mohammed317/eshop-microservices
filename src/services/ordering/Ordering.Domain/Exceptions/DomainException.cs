namespace Ordering.Domain.Exceptions;

/// <summary>
/// Base exception type for domain-layer validation and business rule violations.
/// </summary>
public class DomainException : Exception
{
    // Default constructor
    public DomainException()
    {
    }

    // Constructor that takes a custom error message
    public DomainException(string message)
        : base($"Domain Exception: '{message}' throws from Domain Layer.")
    {
    }

    // Constructor that takes a custom error message and an inner exception
    // Useful for wrapping lower-level exceptions
    public DomainException(string message, Exception innerException)
        : base($"Domain Exception: '{message}' throws from Domain Layer.", innerException)
    {
    }
}