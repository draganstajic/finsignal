using FinSignal.Midlayer.Models;

namespace FinSignal.Midlayer.Reconciliation;

public class InMemoryLedger
{
    public List<InvoiceDto> Invoices { get; } = [];
    public List<PaymentDto> Payments { get; } = [];
}