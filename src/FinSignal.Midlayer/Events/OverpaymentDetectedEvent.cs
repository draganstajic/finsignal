//using FinSignal.Midlayer.Common.Events;
namespace FinSignal.Midlayer.Events;
public class OverpaymentDetectedEvent : BaseEvent
{
    public string InvoiceNumber { get; init; } = default!;
    public decimal OverpaidAmount { get; init; }
}