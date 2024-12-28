namespace Flame.Common.Domain.Exceptions;

/// <summary>
/// Represents an exception for validation errors.
/// </summary>
[Serializable]
public class ValidationException : Exception
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class with the specified validation errors.
    /// </summary>
    /// <param name="errors">The validation errors.</param>
    public ValidationException(IEnumerable<string> errors)
        : base("Validation failed.")
    {
        Errors = errors?.ToList() ?? [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class with the specified error message.
    /// </summary>
    /// <param name="error">The error message.</param>
    public ValidationException(string error)
        : base(error)
    {
        Errors = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class with the specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public ValidationException(string message, Exception inner)
        : base(message, inner)
    {
        Errors = [ inner.Message ];
    }

    #endregion

    /// <summary>
    /// Gets the list of validation errors.
    /// </summary>
    public IReadOnlyList<string> Errors { get; set; }
}