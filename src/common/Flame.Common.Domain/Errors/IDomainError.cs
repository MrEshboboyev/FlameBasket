namespace Flame.Common.Domain.Errors;

/// <summary>
/// Defines the contract for domain errors.
/// </summary>
public interface IDomainError
{
    /// <summary>
    /// Gets the error message.
    /// </summary>
    string? ErrorMessage { get; init; }

    /// <summary>
    /// Gets the type of the error.
    /// </summary>
    ErrorType ErrorType { get; init; }

    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    List<string>? Errors { get; init; }
}