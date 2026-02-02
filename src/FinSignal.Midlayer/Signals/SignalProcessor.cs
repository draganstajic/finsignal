using FinSignal.Midlayer.Models;

namespace FinSignal.Midlayer.Signals;

public class SignalProcessor
{
    private readonly InvoiceStatusStore _store;

    public SignalProcessor(InvoiceStatusStore store)
    {
        _store = store;
    }

    // kada stigne faktura
    public void RegisterInvoice(InvoiceDto invoice)
    {
        _store.GetOrCreate(invoice);
    }

    // kada stigne uplata
    public void RegisterPayment(string reference, decimal amount)
    {
        var status = _store.Get(reference);

        if (status == null)
            return;

        status.ApplyPayment(amount);
    }

    public IReadOnlyCollection<InvoiceStatus> GetAll()
        => _store.All();
}
