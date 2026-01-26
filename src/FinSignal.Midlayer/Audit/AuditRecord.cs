namespace FinSignal.Midlayer.Audit;

public class AuditRecord
{
    public string CorrelationId { get; init; } = default!;
    public string EventType { get; init; } = default!;
    public DateTime OccurredAt { get; init; }
    public object Payload { get; init; } = default!;
}