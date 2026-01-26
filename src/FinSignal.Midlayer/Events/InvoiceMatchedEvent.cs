namespace FinSignal.Midlayer.Events;

public class InvoiceMatchedEvent : BaseEvent
{
    public string InvoiceNumber { get; init; } = default!;
    public string PaymentReference { get; init; } = default!;
    public decimal Amount { get; init; }
}