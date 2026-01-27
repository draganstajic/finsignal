using FinSignal.Midlayer.Events;

namespace FinSignal.Midlayer.Audit;

public interface IAuditStore
{
    void Save(BaseEvent @event);
    IReadOnlyList<AuditRecord> GetByCorrelationId(string correlationId);
}