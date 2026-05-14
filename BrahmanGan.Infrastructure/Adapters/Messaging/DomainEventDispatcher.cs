using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using Microsoft.Extensions.Logging;

namespace BrahmanGan.Infrastructure.Adapters.Messaging;

/// <summary>
/// Implementación del despachador de eventos de dominio
/// </summary>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IEventStore _eventStore;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(
        IEventStore eventStore,
        ILogger<DomainEventDispatcher> logger)
    {
        _eventStore = eventStore;
        _logger = logger;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation(
                "Dispatching domain event: {EventType} - {EventId}",
                domainEvent.GetType().Name,
                domainEvent.EventId);

            // Aquí se pueden agregar handlers adicionales
            // Por ejemplo, publicar a un bus de mensajes

            _logger.LogInformation(
                "Successfully dispatched domain event: {EventType}",
                domainEvent.GetType().Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error dispatching domain event: {EventType}",
                domainEvent.GetType().Name);
            throw;
        }
    }

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken ct = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await DispatchAsync(domainEvent, ct);
        }
    }
}
