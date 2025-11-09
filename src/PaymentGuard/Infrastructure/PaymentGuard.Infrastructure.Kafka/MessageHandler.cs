using Microsoft.Extensions.Hosting;

namespace PaymentGuard.Infrastructure.Kafka;

internal sealed class MessageHandler : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
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