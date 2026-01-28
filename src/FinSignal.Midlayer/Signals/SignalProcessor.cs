using FinSignal.Midlayer.EventBus;
using FinSignal.Midlayer.Events;

namespace FinSignal.Midlayer.Signals;

public class SignalProcessor
{
    private readonly InvoiceStatusStore _store;
    private readonly IEventBus _bus;

    public SignalProcessor(InvoiceStatusStore store, IEventBus bus)
    {
        _store = store;
        _bus = bus;
    }

    public void ProcessMatch(
        string correlationId,
        string invoiceNumber,
        decimal invoiceAmount,
        decimal paidAmount)
    {
        var status = _store.Get(invoiceNumber);
        if (status == null) return;

        status.ApplyPayment(paidAmount);

        if (status.State == InvoiceState.PartiallyPaid)
        {
            _bus.Publish(new PartialPaymentDetectedEvent
            {
                CorrelationId = correlationId,
                EventType = "PartialPaymentDetected",
                Source = "SignalProcessor",
                InvoiceNumber = invoiceNumber,
                Paid = status.PaidAmount,
                Remaining = status.InvoiceAmount - status.PaidAmount
            });
        }
        else if (status.State == InvoiceState.Overpaid)
        {
            _bus.Publish(new OverpaymentDetectedEvent
            {
                CorrelationId = correlationId,
                EventType = "OverpaymentDetected",
                Source = "SignalProcessor",
                InvoiceNumber = invoiceNumber,
                OverpaidAmount = status.PaidAmount - status.InvoiceAmount
            });
        }
    }
}