using Flame.Common.Domain.Extensions;
using Flame.Common.Domain.Primitives;

namespace Flame.Common.Domain.ValueObjects;

/// <summary>
/// Represents a date range value object.
/// </summary>
public sealed class DateRange : ValueObject
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DateRange"/> class with the specified start and end dates.
    /// </summary>
    /// <param name="startDate">The start date of the date range.</param>
    /// <param name="endDate">The end date of the date range.</param>
    private DateRange(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        StartDate = startDate;
        EndDate = endDate.EnsureGreaterThan(startDate);
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a new instance of the <see cref="DateRange"/> class from string representations of the start and end dates.
    /// </summary>
    /// <param name="startDate">The string representation of the start date.</param>
    /// <param name="endDate">The string representation of the end date.</param>
    /// <returns>A new instance of the <see cref="DateRange"/> class.</returns>
    public static DateRange FromString(string startDate, string endDate)
    {
        return new DateRange(DateTimeOffset.Parse(startDate), DateTimeOffset.Parse(endDate));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DateRange"/> class from <see cref="DateTimeOffset"/> representations of the start and end dates.
    /// </summary>
    /// <param name="startDate">The start date of the date range.</param>
    /// <param name="endDate">The end date of the date range.</param>
    /// <returns>A new instance of the <see cref="DateRange"/> class.</returns>
    public static DateRange From(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        return new DateRange(startDate, endDate);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the start date of the date range.
    /// </summary>
    public DateTimeOffset StartDate { get; }

    /// <summary>
    /// Gets the end date of the date range.
    /// </summary>
    public DateTimeOffset EndDate { get; }

    #endregion

    #region Overrides

    /// <summary>
    /// Gets the equality components for the date range.
    /// </summary>
    /// <returns>An enumerable of equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Ensures the specified date is within the date range.
    /// </summary>
    /// <param name="utcNow">The date to check.</param>
    public void InRange(DateTimeOffset utcNow)
    {
        utcNow.EnsureWithinRange(StartDate, EndDate);
    }

    #endregion
}