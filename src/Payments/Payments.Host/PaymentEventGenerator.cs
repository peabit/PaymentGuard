using Payments.Contracts;

namespace Payments;

internal sealed class PaymentEventGenerator(
    PaymentEventRepository eventRepository, 
    PaymentEventPublisher eventPublisher)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.WhenAll(
            GeneratePendingEvents(stoppingToken),
            GenerateProcessingOrCancelledEvents(stoppingToken),
            GenerateCompletedOrCancelledEvents(stoppingToken),
            GenerateRefundedEvents(stoppingToken));
    }

    private async Task GenerateRefundedEvents(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var events = eventRepository.GetEventsWithStatus(PaymentStatus.Completed, limit: 1);

            if (events.Count is not 1)
                continue;

            var newEvent = events[0].ToRefunded();

            eventRepository.UpdateEvent(newEvent);

            await eventPublisher.PublishAsync(newEvent, cancellationToken);

            await Task.Delay(TimeSpan.FromMinutes(2), cancellationToken);
        }
    }

    private async Task GenerateCompletedOrCancelledEvents(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var events = eventRepository.GetEventsWithStatus(PaymentStatus.Processing, limit: 100);

            foreach (var @event in events)
            {
                var newEvent = @event.ToCompletedOrCancelled();

                await RandomDelay(cancellationToken);

                eventRepository.UpdateEvent(newEvent);

                await eventPublisher.PublishAsync(newEvent, cancellationToken);
            }
        }
    }

    private async Task GenerateProcessingOrCancelledEvents(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var events = eventRepository.GetEventsWithStatus(PaymentStatus.Pending, limit: 100);

            foreach (var @event in events)
            {
                var newEvent = @event.ToProcessingOrCancelled();

                await RandomDelay(cancellationToken);

                eventRepository.UpdateEvent(newEvent);

                await eventPublisher.PublishAsync(newEvent, cancellationToken);
            }
        }
    }

    private async Task GeneratePendingEvents(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var @event = PaymentEventFactory.GetPending();

            await RandomDelay(cancellationToken);

            eventRepository.AddEvent(@event);

            await eventPublisher.PublishAsync(@event, cancellationToken);
        }
    }

    private static Task RandomDelay(CancellationToken cancellationToken)
    {
        return Task.Delay(millisecondsDelay: Random.Shared.Next(1, 1001), cancellationToken);
    }
}