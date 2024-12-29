using Flame.Common.Core.Exceptions;
using Flame.Common.Domain.Exceptions;
using MediatR;

namespace Flame.BasketContext.Application.Behaviors;

/// <summary>
/// A pipeline behavior that handles exceptions thrown during request processing.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ExceptionHandlingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    /// <summary>
    /// Handles the given request and captures any exceptions thrown by the next behavior or handler.
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
        try
        {
            // Proceed to the next behavior or actual handler
            return await next();
        }
        catch (ValidationException ex)
        {
            // Handle validation exceptions by returning a DomainError.Validation
            var domainError = DomainError.Validation(ex.Message, ex.Errors.ToList());
            var failureResult = Result.Failure<Guid, IDomainError>(domainError);

            if (failureResult is TResponse response)
            {
                return response;
            }

            throw new InvalidCastException("Failed to cast validation error result " +
                                           "to the expected TResponse type.");
        }
        catch (FlameApplicationException ex)
        {
            // Handle application-specific exceptions
            var domainError = DomainError.BadRequest(ex.Message);
            var failureResult = Result.Failure<Guid, IDomainError>(domainError);

            if (failureResult is TResponse response)
            {
                return response;
            }

            throw new InvalidCastException("Failed to cast bad request error result " +
                                           "to the expected TResponse type.");
        }
        catch (Exception ex)
        {
            // Handle unexpected exceptions by returning a DomainError.Unexpected
            var domainError = DomainError.UnExpected($"An unexpected error occurred: {ex.Message}");
            var failureResult = Result.Failure<Guid, IDomainError>(domainError);

            if (failureResult is TResponse response)
            {
                return response;
            }

            throw new InvalidCastException("Failed to cast unexpected error result " +
                                           "to the expected TResponse type.");
        }
    }
}