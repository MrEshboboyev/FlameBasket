using System.Text.Json;
using Confluent.Kafka;
using Flame.Common.Core.Events;

namespace Flame.BasketContext.Application.Messaging.Integration;

public class IntegrationEventConsumer(
    IConsumer<string, string> consumer,
    Dictionary<Type, object> handlers,
    Dictionary<string, Type> eventTypeMappings)
    : IIntegrationEventConsumer
{
    private readonly IConsumer<string, string> _consumer =
        consumer ?? throw new ArgumentNullException(nameof(consumer));
    private readonly Dictionary<Type, object> _handlers = 
        handlers ?? throw new ArgumentNullException(nameof(handlers)); // Maps event types to their handlers
    private readonly Dictionary<string, Type> _eventTypeMappings = 
        eventTypeMappings ?? throw new ArgumentNullException(nameof(eventTypeMappings)); // Maps topics to event types

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Consume message
                var consumeResult = _consumer.Consume(cancellationToken);

                // Get the event type from the topic
                if (_eventTypeMappings.TryGetValue(consumeResult.Topic, out var eventType))
                {
                    // Deserialize the message into the event type
                    var integrationEvent = JsonSerializer.Deserialize(consumeResult.Message.Value, eventType);

                    if (integrationEvent != null)
                    {
                        // Dispatch the event to the appropriate handler
                        await DispatchEventAsync(integrationEvent, cancellationToken);
                    }
                }
                else
                {
                    Console.WriteLine($"No event type mapping found for topic: {consumeResult.Topic}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Graceful shutdown
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during message consumption: {ex.Message}");
        }
        finally
        {
            _consumer.Close();
        }
    }

    private async Task DispatchEventAsync(
        object integrationEvent,
        CancellationToken cancellationToken)
    {
        var eventType = integrationEvent.GetType();

        // Find the handler for the event type
        if (_handlers.TryGetValue(eventType, out var handler))
        {
            if (handler is IIntegrationEventHandler<IntegrationEvent> typedHandler)
            {
                // Safely cast and invoke the handler
                await typedHandler.HandleAsync((dynamic)integrationEvent, cancellationToken);
            }
            else
            {
                Console.WriteLine($"Handler for {eventType.Name} does not implement the correct interface.");
            }
        }
        else
        {
            Console.WriteLine($"No handler found for event type: {eventType.Name}");
        }
    }
}