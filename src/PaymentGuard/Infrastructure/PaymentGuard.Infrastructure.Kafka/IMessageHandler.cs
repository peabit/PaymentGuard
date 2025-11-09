namespace PaymentGuard.Infrastructure.Kafka;

public interface IMessageHandler<in TMessage>
    where TMessage : class
{
    Task HandleAsync(IReadOnlyList<TMessage> messages, CancellationToken cancellationToken);
}