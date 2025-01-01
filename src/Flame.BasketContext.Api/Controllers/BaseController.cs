using Flame.BasketContext.Api.Extensions;
using Flame.Common.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Flame.BasketContext.Api.Controllers;

[ApiController]
public abstract partial class BaseController : ControllerBase
{
    protected readonly ILogger Logger;

    private readonly Dictionary<ErrorType,
        Func<string?, IEnumerable<string>?, ObjectResult>> _errorHandlers;

    protected BaseController(ILogger logger)
    {
        Logger = logger;
        _errorHandlers = new Dictionary<ErrorType,
            Func<string?, IEnumerable<string>?, ObjectResult>>
        {
            {
                ErrorType.Conflict,
                ConflictResponse
            },
            {
                ErrorType.NotFound,
                NotFoundResponse
            },
            {
                ErrorType.BadRequest,
                BadRequestResponse
            },
            {
                ErrorType.Validation,
                ValidationResponse
            },
            {
                ErrorType.Unexpected,
                UnexpectedResponse
            }
        };
    }

    protected ObjectResult HandleError(IDomainError error)
    {
        if (_errorHandlers.TryGetValue(error.ErrorType, out var handler))
        {
            return handler(
                error.ErrorMessage,
                error.Errors);
        }

        throw new InvalidOperationException($"Unsupported error type: {error.ErrorType}");
    }

    protected ObjectResult NotFoundResponse(
        string? message = null,
        IEnumerable<string>? errors = null) =>
        NotFound(ProblemDetailsFactory.CreateNotFound(
            HttpContext,
            message,
            errors));

    protected ObjectResult BadRequestResponse(
        string? details = null,
        IEnumerable<string>? errors = null) =>
        BadRequest(ProblemDetailsFactory.CreateBadRequest(
            HttpContext,
            details,
            errors));

    protected ObjectResult ConflictResponse(
        string? details = null,
        IEnumerable<string>? errors = null) =>
        Conflict(ProblemDetailsFactory.CreateConflict(
            HttpContext,
            details,
            errors));

    protected ObjectResult ValidationResponse(
        string? details = null,
        IEnumerable<string>? errors = null) =>
        BadRequest(ProblemDetailsFactory.CreateValidation(
            HttpContext,
            details,
            errors));

    protected ObjectResult UnexpectedResponse(
        string? details = null,
        IEnumerable<string>? errors = null) =>
        BadRequest(ProblemDetailsFactory.CreateUnexpectedResponse(
            HttpContext,
            details,
            errors));
}