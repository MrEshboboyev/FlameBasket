using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Flame.BasketContext.Application.Behaviors;

/// <summary>
/// A pipeline behavior that logs the handling process of requests.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class LoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger
        = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Handles the given request and logs the handling process.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the response.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling process started for {request}");
        var metric = Stopwatch.StartNew();
        var response = await next();
        metric.Stop();

        if (metric.Elapsed.Seconds > 5)
            _logger.LogWarning($"Handling process took too much time. " +
                               $"Maybe it needs to be refactored: {metric.Elapsed}");

        _logger.LogInformation($"Handling process done for {request} and you have response {response}");

        return response;
    }
}