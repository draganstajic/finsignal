using System.Text.Json;
using FinSignal.Midlayer.Models;
using FinSignal.Midlayer.Infrastructure;
using System.Runtime.CompilerServices;

namespace FinSignal.Midlayer.Signals;

public class SignalProcessor
{
private readonly IInvoiceStatusRepository _repo;
private readonly HttpClient _http;
private readonly IEventStoreRepository _eventStore;

public SignalProcessor(IInvoiceStatusRepository repo, IEventStoreRepository eventStore)
{
_repo = repo;
_eventStore = eventStore;
_http = new HttpClient();
}

/*public async Task RegisterInvoice(InvoiceDto invoice)
{
var existing = await _repo.GetAsync(invoice.InvoiceNumber);

if (existing == null)
{
var status = new InvoiceStatus(invoice.InvoiceNumber, invoice.Amount);
await _repo.SaveAsync(status);
}
else
{
}
}
*/
// changing for Invoice event store version
public async Task RegisterInvoice(string correlationId, InvoiceDto invoice)
{
    await _eventStore.SaveAsync("InvoiceReceived", correlationId, invoice.InvoiceNumber,invoice);
    var existing = await _repo.GetAsync(invoice.InvoiceNumber);

    if (existing == null)
    {
        var status = new InvoiceStatus(invoice.InvoiceNumber, invoice.Amount);
        await _repo.SaveAsync(status);
    }
}
public async Task RegisterPayment(string correlationId, string reference, decimal amount)
{
var status = await _repo.GetAsync(reference);

if (status == null)
{
var orphan = new
{
Reference = reference,
Amount = amount,
Type = "OrphanPayment"
};

await SendWebhookAsync("orphan", orphan);
return;
}
/*
status.ApplyPayment(amount);
await _repo.SaveAsync(status);
*/
// changing for Payment event store version
await _eventStore.SaveAsync("PaymentReceived", correlationId, reference, new { Reference = reference, Amount = amount });
status.ApplyPayment(amount);
await _repo.SaveAsync(status);
//do ovde
switch (status.State)
{
case InvoiceState.PartiallyPaid:
await SendWebhookAsync("partial", status);
break;

case InvoiceState.Paid:
await SendWebhookAsync("paid", status);
break;

case InvoiceState.Overpaid:
await SendWebhookAsync("overpaid", status);
break;
}
}
// dodajemo za replay eventa
public async Task<InvoiceStatus?> ReplayAsync(string invoiceNumber)
{
    var events = await _eventStore.GetByAggregateIdAsync(invoiceNumber);

    if (!events.Any())
        return null;

    InvoiceStatus? status = null;

    foreach (var evt in events.OrderBy(e => e.OccurredAt))
    {
        switch (evt.SignalType)
        {
            case "InvoiceReceived":
                var invoice = JsonSerializer.Deserialize<InvoiceDto>(evt.Payload);
                status = new InvoiceStatus(invoice!.InvoiceNumber, invoice.Amount);
                break;

            case "PaymentReceived":
                var payment = JsonSerializer.Deserialize<PaymentReplayDto>(evt.Payload);
                status?.ApplyPayment(payment!.Amount);
                break;
        }
    }

    return status;
}


private async Task SendWebhookAsync(string type, object payload)
{
await _http.PostAsJsonAsync(
$"https://sygnal.free.beeceptor.com/{type}",
payload);
}
}