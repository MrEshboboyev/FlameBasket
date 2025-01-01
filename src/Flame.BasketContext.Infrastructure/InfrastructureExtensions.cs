using System.Reflection;
using Flame.BasketContext.Application.Messaging.Integration;
using Flame.BasketContext.Application.Repositories;
using Flame.BasketContext.Domain.Baskets.Services;
using Flame.BasketContext.Infrastructure.Messaging;
using Flame.BasketContext.Infrastructure.Persistence;
using Flame.BasketContext.Infrastructure.Persistence.Repositories;
using Flame.BasketContext.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flame.BasketContext.Infrastructure;

public static class InfrastructureExtensions
{
    public static void ApplyMigrations(this IHost app) 
    {
        using var scope = app.Services.CreateScope();
        
        var services = scope.ServiceProvider;
        
        try
        {
            var context = services.GetRequiredService<BasketAppDbContext>();
            context.Database.Migrate();
            Console.WriteLine("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
            throw;
        }
    }

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        
        #region Services lifecycle
        
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<ISellerLimitService, SellerLimitService>();
        
        #endregion

        // Register AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Get Kafka configuration from appsettings.json
        var kafkaConfig = configuration.GetSection("Kafka");
        var bootstrapServers = kafkaConfig["BootstrapServers"];
        var defaultTopic = kafkaConfig["DefaultTopic"];

        // Register KafkaIntegrationEventPublisher
        services.AddSingleton<IIntegrationEventPublisher>(sp => new KafkaIntegrationEventPublisher(
            bootstrapServers!, defaultTopic!));

        // Register DbContext with a connection string
        services.AddDbContext<BasketAppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

        return services;
    }
}