using MediatR;

namespace Flame.BasketContext.Application.Abstractions;

public interface ICommand : IRequestBase, IRequest<Result<Unit>>
{
    
}

public interface ICommand<TResponse> : IRequestBase, IRequest<Result<TResponse, IDomainError>>
    where TResponse : notnull
{
    
}