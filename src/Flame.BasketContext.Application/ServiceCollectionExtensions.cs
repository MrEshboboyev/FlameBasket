using Flame.BasketContext.Application.Behaviors;
using Flame.BasketContext.Application.Events.Dispatchers;
using Flame.BasketContext.Application.MappingProfiles;
using Flame.Common.Domain.Events;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Flame.BasketContext.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(
                typeof(ServiceCollectionExtensions).Assembly);
            configuration.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
            configuration.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        // Register all domain event handlers
        services.Scan(scan => scan
            .FromAssemblyOf<IDomainEventHandler<IDomainEvent>>()
            .AddClasses(classes =>
                classes.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        services.AddAutoMapper(typeof(BasketMappingProfile));
        
        return services;
    }
}