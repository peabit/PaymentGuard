using System.Collections.Concurrent;
using Payments.Contracts;

namespace Payments;

internal sealed class PaymentEventRepository
{
    private readonly ConcurrentDictionary<Guid, PaymentStatusChanged> _events = new();

    public void AddEvent(PaymentStatusChanged @event)
    {
        var added = _events.TryAdd(@event.PaymentId, @event);

        if (!added)
        {
            throw new InvalidOperationException($"Event for payment with ID {@event.PaymentId} already exists");
        }
    }

    public void UpdateEvent(PaymentStatusChanged @event)
    {
        if (!_events.TryGetValue(@event.PaymentId, out var existing))
        {
            throw new InvalidOperationException($"Event for payment ID {@event.PaymentId} not found");
        }

        var updated = _events.TryUpdate(@event.PaymentId, @event, existing);

        if (!updated)
        {
            throw new InvalidOperationException($"Event for payment ID {@event.PaymentId} not found");
        }
    }

    public IReadOnlyList<PaymentStatusChanged> GetEventsWithStatus(PaymentStatus status, int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);

        return _events.Values
            .Where(e => e.Status == status)
            .Take(limit)
            .ToArray();
    }
}