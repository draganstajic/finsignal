using FinSignal.Midlayer.Models;

namespace FinSignal.Midlayer.Signals;

public class InvoiceStatusStore
{
    private readonly Dictionary<string, InvoiceStatus> _store = new();

    public InvoiceStatus GetOrCreate(InvoiceDto invoice)
    {
        if (!_store.TryGetValue(invoice.InvoiceNumber, out var status))
        {
            status = new InvoiceStatus(invoice.InvoiceNumber, invoice.Amount);
            _store[invoice.InvoiceNumber] = status;
        }

        return status;
    }

    public InvoiceStatus? Get(string invoiceNumber)
    {
        _store.TryGetValue(invoiceNumber, out var status);
        return status;
    }

    public IReadOnlyCollection<InvoiceStatus> All()
        => _store.Values.ToList().AsReadOnly();
}