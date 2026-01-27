using FinSignal.Midlayer.Models;

namespace FinSignal.Midlayer.Events;

public class PaymentReceivedEvent : BaseEvent
{
    public PaymentDto Payment { get; init; } = default!;
}