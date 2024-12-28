using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ValidationExp = Flame.Common.Domain.Exceptions;

namespace Flame.Common.Domain.Extensions;

/// <summary>
/// Provides extension methods for various validation checks.
/// </summary>
public static class Validators
{
    #region Null and Default Checks

    /// <summary>
    /// Creates a new <see cref="ValidationExp.ValidationException"/> with the specified message.
    /// </summary>
    /// <param name="message">The validation error message.</param>
    /// <returns>A new <see cref="ValidationExp.ValidationException"/>.</returns>
    private static ValidationExp.ValidationException Invalid(string message) =>
        new ValidationExp.ValidationException(message);

    /// <summary>
    /// Ensures that the specified model is null.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    /// <param name="model">The model to check.</param>
    /// <param name="name">The name of the model parameter.</param>
    /// <returns>The model if it is null.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the model is not null.</exception>
    public static T? EnsureNull<T>(
        this T? model,
        [CallerArgumentExpression("model")] string name = "")
        => model == null
            ? model
            : throw Invalid($"{name} must be null.");

    /// <summary>
    /// Ensures that the specified model is not null.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    /// <param name="model">The model to check.</param>
    /// <param name="name">The name of the model parameter.</param>
    /// <returns>The model if it is not null.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the model is null.</exception>
    public static T EnsureNonNull<T>(
        this T? model,
        [CallerArgumentExpression("model")] string name = "")
        => model
           ?? throw Invalid($"{name} cannot be null.");

    /// <summary>
    /// Ensures that the specified value is not the default value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="model">The value to check.</param>
    /// <param name="name">The name of the value parameter.</param>
    /// <returns>The value if it is not the default value.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the value is the default value.</exception>
    public static T EnsureNotDefault<T>(
        this T model,
        [CallerArgumentExpression("model")] string name = "")
        where T : struct
    {
        if (model.Equals(default(T)))
            throw Invalid($"{name} cannot be null or default.");
        return model;
    }

    /// <summary>
    /// Ensures that the specified nullable value is not the default value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="model">The nullable value to check.</param>
    /// <param name="name">The name of the value parameter.</param>
    /// <returns>The value if it is not the default value.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the value is the default value.</exception>
    public static T EnsureNotDefault<T>(
        this T? model,
        [CallerArgumentExpression("model")] string name = "")
        where T : struct
    {
        var value = model.GetValueOrDefault();
        if (value.Equals(default(T)))
            throw Invalid($"{name} cannot be null or default.");
        return value;
    }

    #endregion

    #region Numeric Checks

    /// <summary>
    /// Ensures that the specified value is not zero.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="model">The value to check.</param>
    /// <param name="name">The name of the value parameter.</param>
    /// <returns>The value if it is not zero.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the value is zero.</exception>
    public static T EnsureNonZero<T>(
        this T model,
        [CallerArgumentExpression("model")] string name = "")
        where T : struct
        => Convert.ToDecimal(model) != 0 
            ? model 
            : throw Invalid($"{name} cannot be zero.");

    /// <summary>
    /// Ensures that the specified value is positive (greater than zero).
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="model">The value to check.</param>
    /// <param name="name">The name of the value parameter.</param>
    /// <returns>The value if it is positive.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the value is not positive.</exception>
    public static T EnsurePositive<T>(
        this T model,
        [CallerArgumentExpression("model")] string name = "")
        where T : struct
        => model
            .EnsureNonZero(name)
            .EnsureNonNegative(name);

    /// <summary>
    /// Ensures that the specified value is not negative.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="model">The value to check.</param>
    /// <param name="name">The name of the value parameter.</param>
    /// <returns>The value if it is not negative.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the value is negative.</exception>
    public static T EnsureNonNegative<T>(
        this T model,
        [CallerArgumentExpression("model")] string name = "")
        where T : struct
        => Convert.ToDecimal(model) >= 0 
            ? model 
            : throw Invalid($"{name} cannot be negative.");

    /// <summary>
    /// Ensures that the specified value is greater than the minimum value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="model">The value to check.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="modelExpression">The expression of the value parameter.</param>
    /// <param name="minExpression">The expression of the minimum value parameter.</param>
    /// <returns>The value if it is greater than the minimum value.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the value is not greater than the minimum value.</exception>
    public static T EnsureGreaterThan<T>(
        this T model, 
        T min,
        [CallerArgumentExpression("model")] string modelExpression = "",
        [CallerArgumentExpression("min")] string minExpression = "")
        where T : struct, IComparable<T>
        => model.CompareTo(min) > 0
            ? model
            : throw Invalid($"{modelExpression}={model} must be greater than {minExpression}={min}.");

    /// <summary>
    /// Ensures that the specified value is at least the minimum value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="model">The value to check.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="modelExpression">The expression of the value parameter.</param>
    /// <param name="minExpression">The expression of the minimum value parameter.</param>
    /// <returns>The value if it is at least the minimum value.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the value is not at least the minimum value.</exception>
    public static T EnsureAtLeast<T>(
        this T model, 
        T min,
        [CallerArgumentExpression("model")] string modelExpression = "",
        [CallerArgumentExpression("min")] string minExpression = "")
        where T : struct, IComparable<T>
        => model.CompareTo(min) >= 0
            ? model
            : throw Invalid($"{modelExpression}={model} must be greater than or equal to {minExpression}={min}.");

    /// <summary>
    /// Ensures that the specified value is within the specified range.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="model">The value to check.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="excludeMin">Whether to exclude the minimum value.</param>
    /// <param name="excludeMax">Whether to exclude the maximum value.</param>
    /// <param name="modelExpression">The expression of the value parameter.</param>
    /// <param name="minExpression">The expression of the minimum value parameter.</param>        /// <param name="maxExpression">The expression of the maximum value parameter.</param>
    /// <returns>The value if it is within the specified range.</returns>
    /// <exception cref="ValidationExp.ValidationException">Thrown when the value is not within the specified range.</exception>
    public static T EnsureWithinRange<T>(
        this T model,
        T min,
        T max, 
        bool excludeMin = false, 
        bool excludeMax = false,
        [CallerArgumentExpression("model")] string modelExpression = "",
        [CallerArgumentExpression("min")] string minExpression = "",
        [CallerArgumentExpression("max")] string maxExpression = "") 
        where T : struct, IComparable<T>
    {
        var tooLow = excludeMin ? model.CompareTo(min) <= 0 : model.CompareTo(min) < 0;
        var tooHigh = excludeMax ? model.CompareTo(max) >= 0 : model.CompareTo(max) > 0;

        if (tooLow || tooHigh)
            throw Invalid(
                $"{modelExpression}={model} must be between {minExpression}={min} and {maxExpression}={max}.");

        return model;
    }

    #endregion

    #region String Checks

    public static string EnsureNonEmpty(
        this string? model, 
        [CallerArgumentExpression("model")] string name = "")
        => !string.IsNullOrEmpty(model) 
            ? model 
            : throw Invalid($"{name} cannot be empty.");

    public static string EnsureNonBlank(
        this string? model,
        [CallerArgumentExpression("model")] string name = "")
        => !string.IsNullOrWhiteSpace(model)
            ? model 
            : throw Invalid($"{name} cannot be blank.");

    public static string EnsureMatchesPattern(
        this string model,
        string pattern,
        [CallerArgumentExpression("model")] string name = "")
        => Regex.IsMatch(model, pattern)
            ? model 
            : throw Invalid($"{name} does not match the required pattern.");

    public static string EnsureImageUrl(
        this string model,
        [CallerArgumentExpression("model")] string name = "")
        => Regex.IsMatch(
            model,
            "^https?:\\/\\/.*\\/.*\\.(png|gif|webp|jpeg|jpg)\\??.*$")
            ? model
            : throw Invalid($"{name} is not a valid image url.");

    public static string EnsureValidEmail(
        this string email,
        [CallerArgumentExpression("email")] string name = "")
        => new EmailAddressAttribute()
            .IsValid(email)
            ? email 
            : throw Invalid($"{email} is not a valid email address.");

    public static string EnsureExactLength(
        this string model,
        int length,
        [CallerArgumentExpression("model")] string name = "")
        => model.Length == length
            ? model
            : throw Invalid($"{name} must have exactly {length} characters, but it has {model.Length}.");

    public static string EnsureLengthInRange(
        this string model,
        int minLength,
        int maxLength,
        [CallerArgumentExpression("model")] string name = "")
    {
        if (model.Length < minLength || model.Length > maxLength)
            throw Invalid(
                $"{name} length must be between {minLength} and {maxLength} characters, but it has {model.Length}.");
        return model;
    }

    #endregion

    #region Collection Checks

    public static T EnsureNonEmpty<T>(
        this T? collection,
        [CallerArgumentExpression("collection")] string name = "")
        where T : ICollection
        => collection?.Count > 0 
            ? collection
            : throw Invalid($"{name} cannot be empty.");

    #endregion

    #region Enum Checks

    public static T EnsureEnumValueDefined<T>(
        this T model,
        [CallerArgumentExpression("model")] string name = "")
        where T : Enum
        => Enum.IsDefined(typeof(T), model) 
            ? model 
            : throw Invalid($"{name} is not a valid {typeof(T).Name} value.");

    public static T EnsureEnumValueDefined<T>(
        this int model,
        [CallerArgumentExpression("model")] string name = "")
        where T : Enum
        => Enum.IsDefined(typeof(T), model)
            ? (T)Enum.ToObject(typeof(T), model)
            : throw Invalid($"{name}={model} is not a valid {typeof(T).Name} value.");

    public static T EnsureEnumValueDefined<T>(
        this string model,
        [CallerArgumentExpression("model")] string name = "")
        where T : Enum
        => Enum.IsDefined(typeof(T), model)
            ? (T)Enum.Parse(typeof(T), model)
            : throw Invalid($"{name}={model} is not a valid {typeof(T).Name} value.");

    #endregion

    #region Dictionary Checks

    public static TValue EnsureKeyExists<TKey, TValue>(
        this IDictionary<TKey, TValue> model,
        TKey key,
        [CallerArgumentExpression("model")] string name = "")
    {
        model.EnsureNonNull(name);
        key.EnsureNonNull();

        if (!model.TryGetValue(key, out var value))
            throw Invalid($"{key} does not exist inside {name} value.");
        
        return value;
    }

    #endregion

    #region Boolean Checks

    public static bool? EnsureTrue(
        this bool model, 
        [CallerArgumentExpression("model")] string name = "")
        => model == true 
            ? model 
            : throw Invalid($"{name} must be true.");

    public static bool? EnsureFalse(
        this bool model,
        [CallerArgumentExpression("model")] string name = "")
        => model == false 
            ? model 
            : throw Invalid($"{name} must be false.");

    #endregion
}