using FinSignal.Midlayer.EventBus;
using FinSignal.Midlayer.Events;
using FinSignal.Midlayer.Models;
using FinSignal.Midlayer.Reconciliation;
using Microsoft.AspNetCore.Mvc;
using FinSignal.Midlayer.Signals;


namespace FinSignal.Midlayer.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IEventBus _eventBus;
    private readonly MatchingEngine _matching;
    private readonly SignalProcessor _signals;

    public InvoiceController(IEventBus eventBus, MatchingEngine matching, SignalProcessor signals)
{
    _eventBus = eventBus;
    _matching = matching;
    _signals = signals;
}

    [HttpPost]
    public IActionResult Receive([FromBody] InvoiceDto invoice)
    {
        var correlationId = Guid.NewGuid().ToString();
        _signals.RegisterInvoice(invoice);

        var ev = new InvoiceReceivedEvent
        {
            CorrelationId = correlationId,
            EventType = "InvoiceReceived",
            Source = "eFaktura",
            Invoice = invoice
        };

        _eventBus.Publish(ev);
        _matching.RegisterInvoice(correlationId, invoice);

        return Ok(new { correlationId });
    }
}
