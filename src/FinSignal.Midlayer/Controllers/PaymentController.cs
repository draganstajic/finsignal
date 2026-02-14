using FinSignal.Midlayer.EventBus;
using FinSignal.Midlayer.Events;
using FinSignal.Midlayer.Models;
using FinSignal.Midlayer.Reconciliation;
using Microsoft.AspNetCore.Mvc;
using FinSignal.Midlayer.Signals;


namespace FinSignal.Midlayer.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IEventBus _bus;
    private readonly MatchingEngine _matching;
    private readonly SignalProcessor _signals;


    public PaymentController(IEventBus bus, MatchingEngine matching, SignalProcessor signals)
{
    _bus = bus;
    _matching = matching;
    _signals = signals;
}

    [HttpPost]
    public async Task<IActionResult> Receive([FromBody] PaymentDto payment)
    {
        var correlationId = Guid.NewGuid().ToString();
        await _signals.RegisterPayment(payment.PaymentReference, payment.Amount);

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
