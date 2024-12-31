using MediatR;

namespace Flame.BasketContext.Application.Abstractions.Queries;

public interface IQueryHandler<in TRequest, TResponse> 
    : IRequestHandler<TRequest, Result<TResponse, IDomainError>>
    where TRequest : IQuery<TResponse>
    where TResponse : notnull
{
    
}