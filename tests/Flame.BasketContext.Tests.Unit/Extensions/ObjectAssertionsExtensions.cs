using Flame.Common.Domain.Events;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Flame.BasketContext.Tests.Unit.Extensions;

public static class ObjectAssertionsExtensions
{
    public static AndConstraint<ObjectAssertions> BeEquivalentEventTo<TExpectation>(
        this ObjectAssertions obj,
        TExpectation expectation, 
        string because = "",
        params object[] becauseArgs)
        where TExpectation : IDomainEvent
    {
        return obj.BeEquivalentTo(expectation,
            options => options
                .Excluding(t => t.OccurredOnUtc)
                .Excluding(t => t.Id));
    }
}