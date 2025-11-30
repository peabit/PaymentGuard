using PaymentGuard.Infrastructure.Kafka;

namespace Host;

public record MyMessage();

public class MyHandler(ILogger<MyHandler> logger) : IMessageHandler<MyMessage>
{
    public Task HandleAsync(IReadOnlyList<MyMessage> messages, CancellationToken cancellationToken)
    {
        logger.LogInformation("Hello!!!");
        
        return Task.CompletedTask;
    }
}