namespace BrahmanGan.Infrastructure.Adapters.EventSourcing;

/// <summary>
/// Modelo de persistencia para eventos de dominio
/// </summary>
public class StoredEvent
{
    public int Id { get; set; }
    public Guid EventId { get; set; }
    public string AggregateType { get; set; } = string.Empty;
    public int AggregateId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty;
    public DateTime OccurredOn { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
