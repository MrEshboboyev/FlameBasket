namespace Flame.Common.Domain.Exceptions;

/// <summary>
/// Represents an exception for eventual consistency errors.
/// </summary>
public class EventualConsistencyException : Exception
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EventualConsistencyException"/> class with the specified error code and error message.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="details">Additional details about the error.</param>
    public EventualConsistencyException(
        string errorCode,
        string errorMessage,
        List<string>? details = null)
        : base(errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Details = details ?? [];
    }

    #endregion

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Gets the list of additional details about the error.
    /// </summary>
    public List<string> Details { get; }
}