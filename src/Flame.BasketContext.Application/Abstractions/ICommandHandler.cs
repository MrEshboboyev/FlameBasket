﻿using MediatR;

namespace Flame.BasketContext.Application.Abstractions;

public interface ICommandHandler<in TRequest> 
    : IRequestHandler<TRequest, Result<Unit>>
    where TRequest : ICommand
{
    
}

public interface ICommandHandler<in TRequest, TResponse> 
    : IRequestHandler<TRequest, Result<TResponse, IDomainError>>
    where TRequest : ICommand<TResponse>
    where TResponse : notnull
{
    
}