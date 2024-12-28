namespace Flame.Common.Core.Exceptions;

/// <summary>
/// Represents application-specific exceptions for the Flame application.
/// </summary>
[Serializable]
public class FlameApplicationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlameApplicationException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public FlameApplicationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlameApplicationException"/> class with the specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public FlameApplicationException(string message, Exception inner) : base(message, inner)
    {
    }
}