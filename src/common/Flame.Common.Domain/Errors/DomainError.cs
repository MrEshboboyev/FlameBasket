namespace Flame.Common.Domain.Errors;

/// <summary>
/// Represents a domain error.
/// </summary>
public record DomainError : IDomainError
{
    #region Constructor and implementation

    // Constructor is private to enforce the usage of static methods
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainError"/> class with the specified parameters.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errorType">The type of the error.</param>
    /// <param name="errors">The list of errors.</param>
    private DomainError(
        string? message,
        ErrorType errorType,
        List<string>? errors = null)
    {
        ErrorMessage = message;
        ErrorType = errorType;
        Errors = errors ?? [];
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Gets the type of the error.
    /// </summary>
    public ErrorType ErrorType { get; init; }

    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    public List<string>? Errors { get; init; }

    #endregion

    #region Static factory methods

    /// <summary>
    /// Creates a new conflict error.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new instance of <see cref="DomainError"/>.</returns>
    public static DomainError Conflict(
        string? message = "The data provided conflicts with existing data.") =>
        new DomainError(
            message ?? "The data provided conflicts with existing data.",
            ErrorType.Conflict);

    /// <summary>
    /// Creates a new not found error.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new instance of <see cref="DomainError"/>.</returns>
    public static DomainError NotFound(
        string? message = "The requested item could not be found.") =>
        new DomainError(
            message ?? "The requested item could not be found.",
            ErrorType.NotFound);

    /// <summary>
    /// Creates a new bad request error.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new instance of <see cref="DomainError"/>.</returns>
    public static DomainError BadRequest(
        string? message = "Invalid request or parameters.") =>
        new DomainError(
            message ?? "Invalid request or parameters.", 
            ErrorType.BadRequest);

    /// <summary>
    /// Creates a new validation error.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The list of validation errors.</param>
    /// <returns>A new instance of <see cref="DomainError"/>.</returns>
    public static DomainError Validation(
        string? message = "Validation Failed.",
        List<string>? errors = null) =>
        new DomainError(
            message ?? "Validation Failed.",
            ErrorType.Validation,
            errors);

    /// <summary>
    /// Creates a new unexpected error.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new instance of <see cref="DomainError"/>.</returns>
    public static DomainError UnExpected(
        string? message = "Unexpected error happened.") =>
        new DomainError(
            message ?? "Something went wrong.", 
            ErrorType.Unexpected);

    #endregion
}