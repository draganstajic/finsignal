using FinSignal.Midlayer.Data.Entities;

namespace FinSignal.Midlayer.Infrastructure;

public interface IEventStoreRepository
{
    Task SaveAsync(string signalType, string correlationId, string aggregateId, object payload);
    Task<List<SignalEvent>> GetByCorrelationIdAsync(string correlationId);// dodajemo metodu da vrati sve evente za navedeni tok
    Task<List<SignalEvent>> GetByAggregateIdAsync(string aggregateId);
}
