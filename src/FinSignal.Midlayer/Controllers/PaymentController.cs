using FinSignal.Midlayer.EventBus;
using FinSignal.Midlayer.Events;
using FinSignal.Midlayer.Models;
using FinSignal.Midlayer.Reconciliation;
using Microsoft.AspNetCore.Mvc;

namespace FinSignal.Midlayer.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IEventBus _bus;
    private readonly MatchingEngine _matching;

    public PaymentController(IEventBus bus, MatchingEngine matching)
    {
        _bus = bus;
        _matching = matching;
    }

    [HttpPost]
    public IActionResult Receive([FromBody] PaymentDto payment)
    {
        var correlationId = Guid.NewGuid().ToString();

        var ev = new PaymentReceivedEvent
        {
            CorrelationId = correlationId,
            EventType = "PaymentReceived",
            Source = "Bank",
            Payment = payment
        };

        _bus.Publish(ev);
        _matching.RegisterPayment(correlationId, payment);

        return Ok(new { correlationId });
    }
}
