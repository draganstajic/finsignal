namespace FinSignal.Midlayer.Events;

public class MismatchDetectedEvent : BaseEvent
{
    public string Reason { get; init; } = default!;
}