using FinSignal.Midlayer.Events;

namespace FinSignal.Midlayer.EventBus;

public interface IEventBus
{
    void Publish(BaseEvent @event);
}