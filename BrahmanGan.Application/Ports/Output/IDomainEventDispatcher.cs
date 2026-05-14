using BrahmanGan.Domain.Common;

namespace BrahmanGan.Application.Ports.Output;

/// <summary>
/// Puerto de salida para Domain Event Dispatcher
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken ct = default);
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct = default);
}
