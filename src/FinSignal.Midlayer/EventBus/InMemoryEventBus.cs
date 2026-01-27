using FinSignal.Midlayer.Audit;
using FinSignal.Midlayer.Events;

namespace FinSignal.Midlayer.EventBus;

public class InMemoryEventBus : IEventBus
{
    private readonly IAuditStore _audit;

    public InMemoryEventBus(IAuditStore audit)
    {
        _audit = audit;
    }

    public void Publish(BaseEvent @event)
    {
        _audit.Save(@event);
    }
}