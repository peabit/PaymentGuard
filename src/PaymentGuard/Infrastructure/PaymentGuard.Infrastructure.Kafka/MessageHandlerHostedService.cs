using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PaymentGuard.Infrastructure.Kafka;

internal sealed class MessageHandlerHostedService<TMessage>(
    IMessageHandler<TMessage> handler) 
    : BackgroundService
//  MessageConsumer<TMessage> consumer consumer.Commit();
//  BatchFactory
//  DateTimeProvider
    where TMessage : class
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await handler.HandleAsync([], stoppingToken);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}

// internal sealed class MessageHandler<TMessage>(
//     ITopicNameResolver topicNameResolver, 
//     BrokerConfig brokerConfig,
//     BatchConfig batchConfig,
//     DateTimeProvider dateTimeProvider)
//     : BackgroundService
// {
//     protected override Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         var topic = topicNameResolver.Resolve<TMessage>();
//
//         var batch = new Batch<TMessage>(batchConfig);
//
//         while()
//         {
//             var rawMessage = ...
//
//             var message = JsonSerializer.Deserialise<TMessage>(rawMessage.Message.Value);
//
//             batch.Add(message, dateTimeProvider.UtcNow);
//
//             if (batch.CanHandle)
//             {
//                 var messages = batch.TakeAll(dateTimeProvider.UtcNow);
//                 
//                 await Handle(messages, cancellationToken);
//             }
//         }
//     }
//
//     private async Task Handle(IReadOnlyList<TMessage> messages, CancellationToken cancellationToken) 
//     {
//         // scope
//         // var handler = scope.GetRequiredService<IMessageHandler<T>>();
//
//         await handler.HandleAsync(messages, cancellationToken);
//     }
// }