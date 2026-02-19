using System.Text.Json;
using FinSignal.Midlayer.Data;
using FinSignal.Midlayer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinSignal.Midlayer.Infrastructure;



public class EventStoreRepository : IEventStoreRepository
{
    private readonly FinSignalDbContext _context;

    public EventStoreRepository(FinSignalDbContext context)
    {
        _context = context;
    }
// vraca sve evente iz baze za trazeni tok
    public async Task<List<SignalEvent>> GetByCorrelationIdAsync(string correlationId)
{
    return await _context.SignalEvents
        .Where(e => e.CorrelationId == correlationId)
        .OrderBy(e => e.OccurredAt)
        .ToListAsync();
}
public async Task<List<SignalEvent>> GetByAggregateIdAsync(string aggregateId)
{
    return await _context.SignalEvents
        .Where(e => e.AggregateId == aggregateId)
        .OrderBy(e => e.OccurredAt)
        .ToListAsync();
}

    public async Task SaveAsync(string signalType, string correlationId, string aggregateId, object payload)
    {
        var entity = new SignalEvent
        {
            Id = Guid.NewGuid(),
            SignalType = signalType,
            CorrelationId = correlationId,
            AggregateId = aggregateId, // postavljamo identifikator agregata
            Payload = JsonSerializer.Serialize(payload),
            OccurredAt = DateTime.UtcNow
        };

        _context.SignalEvents.Add(entity);
        await _context.SaveChangesAsync();
    }
}
