using FinSignal.Midlayer.Models;

namespace FinSignal.Midlayer.Events;

public class InvoiceReceivedEvent : BaseEvent
{
    public InvoiceDto Invoice { get; init; } = default!;
}