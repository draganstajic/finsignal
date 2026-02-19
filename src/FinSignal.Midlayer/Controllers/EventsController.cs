using FinSignal.Midlayer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using FinSignal.Midlayer.Models;
using FinSignal.Midlayer.Signals;

namespace FinSignal.Midlayer.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventStoreRepository _eventStore;
    private readonly SignalProcessor _signalProcessor;

    public EventsController(IEventStoreRepository eventStore, SignalProcessor signalProcessor)
    {
        _eventStore = eventStore;
        _signalProcessor = signalProcessor;
    }

    [HttpGet("{correlationId}")]
    public async Task<IActionResult> Get(string correlationId)
    {
        var events = await _eventStore.GetByCorrelationIdAsync(correlationId);

        return Ok(events);
    }

    [HttpGet("aggregate/{aggregateId}")]
    public async Task<IActionResult> GetByAggregateId(string aggregateId)
    {
        var events = await _eventStore.GetByAggregateIdAsync(aggregateId);

        return Ok(events);
    }

    [HttpGet("replay/{invoiceNumber}")]
public async Task<IActionResult> Replay(string invoiceNumber)
{
    var result = await _signalProcessor.ReplayAsync(invoiceNumber);

    if (result == null)
        return NotFound();

    return Ok(result);
}
}
