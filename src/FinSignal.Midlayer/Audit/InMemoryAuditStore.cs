using FinSignal.Midlayer.Events;

namespace FinSignal.Midlayer.Audit;

public class InMemoryAuditStore : IAuditStore
{
    private readonly List<AuditRecord> _records = [];

    public void Save(BaseEvent @event)
    {
        _records.Add(new AuditRecord
        {
            CorrelationId = @event.CorrelationId,
            EventType = @event.EventType,
            OccurredAt = @event.OccurredAt,
            Payload = @event
        });
    }

    public IReadOnlyList<AuditRecord> GetByCorrelationId(string correlationId)
    {
        return _records
            .Where(r => r.CorrelationId == correlationId)
            .OrderBy(r => r.OccurredAt)
            .ToList();
    }
}