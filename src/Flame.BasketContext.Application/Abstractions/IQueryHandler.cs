using MediatR;

namespace Flame.BasketContext.Application.Abstractions;

public interface IQueryHandler<in TRequest, TResponse> 
    : IRequestHandler<TRequest, Result<TResponse, IDomainError>>
    where TRequest : IQuery<TResponse>
    where TResponse : IDomainError
{
    
}