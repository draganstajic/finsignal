namespace FinSignal.Midlayer.Events;

public class PartialPaymentDetectedEvent : BaseEvent
{
    public string InvoiceNumber { get; init; } = default!;
    public decimal Paid { get; init; }
    public decimal Remaining { get; init; }
}