using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.Exceptions; // الـ Domain Exception بتاعك

namespace Ordering.API.Exceptions;

public class OrderingExceptionHandler(ILogger<OrderingExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DomainException domainException)
        {
            return false;
        }

        logger.LogError("Ordering Domain Error: {exceptionMessage}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "Domain Rule Violation",
            Detail = domainException.Message,
            Status = StatusCodes.Status400BadRequest,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("TraceId", context.TraceIdentifier);

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}