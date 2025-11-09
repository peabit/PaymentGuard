namespace Payments.Contracts;

public sealed record PaymentStatusChanged(
    Guid PaymentId,
    string OrderNumber,
    string BusinessType,
    DateTimeOffset DateTime,
    PaymentStatus Status);