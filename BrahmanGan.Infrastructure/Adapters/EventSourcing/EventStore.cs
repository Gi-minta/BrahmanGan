using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;

namespace BrahmanGan.Infrastructure.Adapters.EventSourcing;

/// <summary>
/// Implementación del Event Store
/// </summary>
public class EventStore : IEventStore
{
    private readonly EventStoreDbContext _context;

    public EventStore(EventStoreDbContext context)
    {
        _context = context;
    }

    public async Task SaveEventAsync(
        IDomainEvent domainEvent,
        string aggregateType,
        int aggregateId,
        int version,
        CancellationToken ct = default)
    {
        var storedEvent = new StoredEvent
        {
            EventId = domainEvent.EventId,
            AggregateType = aggregateType,
            AggregateId = aggregateId,
            EventType = domainEvent.GetType().Name,
            EventData = SerializeEvent(domainEvent),
            OccurredOn = domainEvent.OccurredOn,
            Version = version
        };

        await _context.Events.AddAsync(storedEvent, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<IDomainEvent>> GetEventsAsync(
        string aggregateType,
        int aggregateId,
        CancellationToken ct = default)
    {
        var storedEvents = await _context.Events
            .Where(e => e.AggregateType == aggregateType && e.AggregateId == aggregateId)
            .OrderBy(e => e.Version)
            .ToListAsync(ct);

        return storedEvents.Select(DeserializeEvent).Where(e => e != null)!;
    }

    public async Task<IEnumerable<IDomainEvent>> GetAllEventsAsync(CancellationToken ct = default)
    {
        var storedEvents = await _context.Events
            .OrderBy(e => e.OccurredOn)
            .ToListAsync(ct);

        return storedEvents.Select(DeserializeEvent).Where(e => e != null)!;
    }

    private static string SerializeEvent(IDomainEvent domainEvent)
    {
        return JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }

    private static IDomainEvent? DeserializeEvent(StoredEvent storedEvent)
    {
        try
        {
            return JsonConvert.DeserializeObject<IDomainEvent>(storedEvent.EventData, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
        catch
        {
            // Log error
            return null;
        }
    }
}
