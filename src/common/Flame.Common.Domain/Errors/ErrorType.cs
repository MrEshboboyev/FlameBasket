﻿using Ardalis.SmartEnum;

namespace Flame.Common.Domain.Errors;

/// <summary>
/// Represents the type of error.
/// </summary>
public abstract class ErrorType(string name, int value) 
    : SmartEnum<ErrorType>(name, value)
{
    #region Private inheritors
    
    private class ConflictEnum() : ErrorType("Conflict", 0);
    private class NotFoundEnum() : ErrorType("NotFound", 1);
    private class BadRequestEnum() : ErrorType("BadRequest", 2);
    private class ValidationEnum() : ErrorType("Validation", 3);
    private class UnexpectedEnum() : ErrorType("Unexpected", 4);
    
    #endregion
    
    public static readonly ErrorType Conflict = new ConflictEnum();
    public static readonly ErrorType NotFound = new NotFoundEnum();
    public static readonly ErrorType BadRequest = new BadRequestEnum();
    public static readonly ErrorType Validation = new ValidationEnum();
    public static readonly ErrorType Unexpected = new UnexpectedEnum();
}