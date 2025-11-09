using Payments.Contracts;

namespace Payments;

internal static class PaymentEventFactory
{
    public static PaymentStatusChanged GetPending()
    {
        var businessType = Random.Shared.NextDouble() switch
        {
            < 0.5 => "OnlineShop",          
            < 0.6666 => "Tips",             
            < 0.8333 => "Installment",     
            _ => "Subscription"            
        };

        var orderNumber = $"{Random.Shared.Next(1, 100_000_000)}-{Random.Shared.Next(1, 1_000_000)}";

        return new PaymentStatusChanged(
            PaymentId: Guid.NewGuid(),
            orderNumber,
            businessType,
            DateTimeOffset.UtcNow,
            PaymentStatus.Pending);
    }

    public static PaymentStatusChanged ToProcessingOrCancelled(this PaymentStatusChanged @event)
    {
        var status = Random.Shared.NextDouble() < 0.9
            ? PaymentStatus.Processing
            : PaymentStatus.Cancelled;

        return @event.NextStaus(status);
    }
    
    public static PaymentStatusChanged ToCompletedOrCancelled(this PaymentStatusChanged @event)
    {
        var status = Random.Shared.NextDouble() < 0.9
            ? PaymentStatus.Completed
            : PaymentStatus.Cancelled;

        return @event.NextStaus(status);
    }

    public static PaymentStatusChanged ToRefunded(this PaymentStatusChanged @event)
        => @event.NextStaus(PaymentStatus.Refunded);

    private static PaymentStatusChanged NextStaus(this PaymentStatusChanged @event, PaymentStatus status)
        => @event with
        {
            Status = status,
            DateTime = DateTimeOffset.UtcNow
        };
}