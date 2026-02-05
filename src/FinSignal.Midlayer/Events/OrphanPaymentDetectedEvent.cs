namespace FinSignal.Midlayer.Events;

public class OrphanPaymentDetectedEvent
{
    public string PaymentReference { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
}