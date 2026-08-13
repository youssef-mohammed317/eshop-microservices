using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> _logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var responseName = typeof(TResponse).Name;

        _logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            requestName, responseName, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;

        // Log a warning if the request takes longer than 3 seconds
        if (timeTaken.Seconds > 3)
        {
            _logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                requestName, timeTaken.Seconds);
        }

        _logger.LogInformation("[END] Handled {Request} with {Response}",
            requestName, responseName);

        return response;
    }
}