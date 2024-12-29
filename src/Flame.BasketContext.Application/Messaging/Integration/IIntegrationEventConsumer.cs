namespace Flame.BasketContext.Application.Messaging.Integration;

public interface IIntegrationEventConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}
