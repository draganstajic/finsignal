namespace FinSignal.Midlayer.Data.Entities;
public class SignalEvent
/*{
    public Guid Id { get; set; }
    public string SignalType { get; set; } = default;
    public string CorelationId { get; set; } = default;
    public string Payload { get; set; } = default;
    public DateTime OccurredAt { get; set; }
}*/

{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SignalType { get; set; } = default!;
    public string CorrelationId { get; set; } = default!;
    public string AggregateId { get; set; } = default!; // dodajemo polje za identifikaciju agregata
    public string Payload { get; set; } = default!;
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}