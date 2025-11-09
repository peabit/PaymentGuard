namespace Orders.Contracts;

public sealed record OrderPaid(
    string OrderNumber, 
    Guid PaymentId,
    DateTimeOffset DateTime);