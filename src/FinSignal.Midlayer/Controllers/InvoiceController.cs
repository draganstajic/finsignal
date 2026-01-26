using FinSignal.Midlayer.EventBus;
using FinSignal.Midlayer.Events;
using FinSignal.Midlayer.Models;
using FinSignal.Midlayer.Reconciliation;
using Microsoft.AspNetCore.Mvc;

namespace FinSignal.Midlayer.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IEventBus _eventBus;
    private readonly MatchingEngine _matching;

    public InvoiceController(IEventBus eventBus, MatchingEngine matching)
    {
        _eventBus = eventBus;
        _matching = matching;
    }

    [HttpPost]
    public IActionResult Receive([FromBody] InvoiceDto invoice)
    {
        var correlationId = Guid.NewGuid().ToString();

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
