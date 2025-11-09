using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Orders.Contracts;
using Payments.Contracts;

namespace Orders;

internal sealed class Consumer : BackgroundService
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = CreateConsumer();

        using var producer = CreateProducer();

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = consumer.Consume(stoppingToken);

            if (result is null)
                continue;

            var paymentEvent = 
                JsonSerializer.Deserialize<PaymentStatusChanged>(result.Message.Value, _serializerOptions)!;

            var orderPaid = paymentEvent is
            {
                BusinessType: "OnlineShop",
                Status: PaymentStatus.Completed
            };
            
            if (!orderPaid)
                continue;
            
            var orderEvent = new OrderPaid(
                paymentEvent.OrderNumber,
                paymentEvent.PaymentId,
                DateTimeOffset.UtcNow);

            var message = new Message<string, string>()
            {
                Key = orderEvent.PaymentId.ToString(),
                Value = JsonSerializer.Serialize(orderEvent, _serializerOptions)
            };

            await producer.ProduceAsync(topic: "order-paid", message, stoppingToken);
        }
    }

    private static IConsumer<Ignore, string> CreateConsumer()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "orders",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

        consumer.Subscribe("payment-status-changed");

        return consumer;
    }

    private static IProducer<string, string> CreateProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
        };
        
        return new ProducerBuilder<string, string>(config).Build();
    }
}