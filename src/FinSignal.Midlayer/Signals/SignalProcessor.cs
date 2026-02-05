using System.Net.Http.Json;
using FinSignal.Midlayer.Models;

namespace FinSignal.Midlayer.Signals;

public class SignalProcessor
{
    private readonly InvoiceStatusStore _store = new();
    private readonly List<object> _orphans = new();
    private readonly HttpClient _http = new();

    // ===============================
    // INVOICE
    // ===============================
    public void RegisterInvoice(InvoiceDto invoice)
    {
        _store.GetOrCreate(invoice);
    }

    // ===============================
    // PAYMENT
    // ===============================
    public void RegisterPayment(string reference, decimal amount)
    {
        var status = _store.Get(reference);

        // ORPHAN
        if (status == null)
        {
            var orphan = new
            {
                Reference = reference,
                Amount = amount,
                Type = "OrphanPayment"
            };

            _orphans.Add(orphan);

            _ = SendWebhookAsync("orphan", orphan);

            return;
        }

        // NORMAL FLOW
        status.ApplyPayment(amount);

        switch (status.State)
        {
            case InvoiceState.PartiallyPaid:
                _ = SendWebhookAsync("partial", status);
                break;

            case InvoiceState.Paid:
                _ = SendWebhookAsync("paid", status);
                break;

            case InvoiceState.Overpaid:
                _ = SendWebhookAsync("overpaid", status);
                break;
        }
    }

    // ===============================
    // WEBHOOK
    // ===============================
    private async Task SendWebhookAsync(string type, object payload)
    {
        await _http.PostAsJsonAsync(
            $"https://sygnal.free.beeceptor.com/{type}",
            payload);
    }
}
