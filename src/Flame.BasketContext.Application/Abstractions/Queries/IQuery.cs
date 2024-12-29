using MediatR;

namespace Flame.BasketContext.Application.Abstractions.Queries;

public interface IQuery<TResponse> : IRequestBase, IRequest<Result<TResponse, IDomainError>>
    where TResponse : notnull
{
    
}

