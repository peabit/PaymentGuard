using Confluent.Kafka;

namespace Host;

internal sealed class KafkaListener(ILogger<KafkaListener> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "foo",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("my-topic");
        
        logger.LogInformation("Start!!!");

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);

            if (consumeResult is not null)
            {
                logger.LogInformation(consumeResult.Message.Value);
            }
        }

        consumer.Close();
    }
}