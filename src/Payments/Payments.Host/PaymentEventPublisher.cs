using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Payments.Contracts;

namespace Payments;

internal sealed class PaymentEventPublisher : IDisposable
{
    private readonly IProducer<string, string> _producer;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public PaymentEventPublisher()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync(PaymentStatusChanged @event, CancellationToken cancellationToken)
    {
        var message = new Message<string, string>
        {
            Key = @event.PaymentId.ToString(),
            Value = JsonSerializer.Serialize(@event, _serializerOptions)
        };

        await _producer.ProduceAsync(topic: "payment-status-changed", message, cancellationToken);
    }

    public void Dispose() => _producer.Dispose();
}