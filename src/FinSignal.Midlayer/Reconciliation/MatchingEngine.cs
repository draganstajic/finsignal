using FinSignal.Midlayer.EventBus;
using FinSignal.Midlayer.Events;
using FinSignal.Midlayer.Models;

namespace FinSignal.Midlayer.Reconciliation;

public class MatchingEngine
{
    private readonly InMemoryLedger _ledger;
    private readonly IEventBus _bus;

    public MatchingEngine(InMemoryLedger ledger, IEventBus bus)
    {
        _ledger = ledger;
        _bus = bus;
    }

    public void RegisterInvoice(string correlationId, InvoiceDto invoice)
    {
        _ledger.Invoices.Add(invoice);
        TryMatch(correlationId);
    }

    public void RegisterPayment(string correlationId, PaymentDto payment)
    {
        _ledger.Payments.Add(payment);
        TryMatch(correlationId);
    }

    private void TryMatch(string correlationId)
    {
        foreach (var inv in _ledger.Invoices)
        {
            var payment = _ledger.Payments
                .FirstOrDefault(p => p.Amount == inv.Amount && p.Currency == inv.Currency);

            if (payment != null)
            {
                _bus.Publish(new InvoiceMatchedEvent
                {
                    CorrelationId = correlationId,
                    EventType = "InvoiceMatched",
                    Source = "MatchingEngine",
                    InvoiceNumber = inv.InvoiceNumber,
                    PaymentReference = payment.PaymentReference,
                    Amount = inv.Amount
                });

                _ledger.Invoices.Remove(inv);
                _ledger.Payments.Remove(payment);
                return;
            }
        }
    }
}