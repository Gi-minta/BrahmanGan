using BrahmanGan.Domain.Common;

namespace BrahmanGan.Application.Ports.Output;

/// <summary>
/// Puerto de salida para Event Store
/// </summary>
public interface IEventStore
{
    Task SaveEventAsync(IDomainEvent domainEvent, string aggregateType, int aggregateId, int version, CancellationToken ct = default);
    Task<IEnumerable<IDomainEvent>> GetEventsAsync(string aggregateType, int aggregateId, CancellationToken ct = default);
    Task<IEnumerable<IDomainEvent>> GetAllEventsAsync(CancellationToken ct = default);
}
