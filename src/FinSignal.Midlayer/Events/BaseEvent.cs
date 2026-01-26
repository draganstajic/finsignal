namespace FinSignal.Midlayer.Events;

public abstract class BaseEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string CorrelationId { get; init; } = default!;
    public string EventType { get; init; } = default!;
    public string Source { get; init; } = default!;
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}