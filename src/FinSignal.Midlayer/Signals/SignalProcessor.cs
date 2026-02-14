using System.Net.Http.Json;
using FinSignal.Midlayer.Models;

namespace FinSignal.Midlayer.Signals;

public class SignalProcessor
{
private readonly IInvoiceStatusRepository _repo;
private readonly HttpClient _http;

public SignalProcessor(IInvoiceStatusRepository repo)
{
_repo = repo;
_http = new HttpClient();
}

public async Task RegisterInvoice(InvoiceDto invoice)
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

public async Task RegisterPayment(string reference, decimal amount)
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

status.ApplyPayment(amount);
await _repo.SaveAsync(status);


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

private async Task SendWebhookAsync(string type, object payload)
{
await _http.PostAsJsonAsync(
$"https://sygnal.free.beeceptor.com/{type}",
payload);
}
}