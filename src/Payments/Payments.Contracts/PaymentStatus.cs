namespace Payments.Contracts;

public enum PaymentStatus
{
    Pending,           
    Processing,
    Completed,       
    Cancelled,       
    Refunded
}