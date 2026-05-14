namespace BrahmanGan.Domain.Common;

/// <summary>Marca una entidad capaz de emitir eventos de dominio.</summary>
public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

/// <summary>Clase base para todas las entidades del dominio.</summary>
public abstract class Entity<TId> : IHasDomainEvents where TId : class
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public TId Id { get; protected set; } = null!;
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected internal void AddDomainEvent(IDomainEvent eventItem) => _domainEvents.Add(eventItem);
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void MarkAsModified() => UpdatedAt = DateTime.UtcNow;
}
