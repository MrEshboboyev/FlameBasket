using FluentValidation;
using MediatR;
using Validation = Flame.Common.Domain.Exceptions;

namespace Flame.BasketContext.Application.Behaviors;

/// <summary>
/// A pipeline behavior that validates requests using FluentValidation validators.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators =
        validators ?? throw new ArgumentNullException(nameof(validators));

    /// <summary>
    /// Handles the given request and performs validation using the provided validators.
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
        if (!_validators.Any()) return await next();

        var validationContext = new ValidationContext<TRequest>(request);
        var validationResponse = await Task
            .WhenAll(_validators
                .Select(x => x.ValidateAsync(validationContext, cancellationToken)));

        var validationErrors = validationResponse
            .SelectMany(x => x.Errors)
            .Where(e => e != null)
            .Select(x => x.ErrorMessage)
            .ToList();

        if (validationErrors.Count != 0) 
            throw new Validation.ValidationException(validationErrors);

        return await next();
    }
}