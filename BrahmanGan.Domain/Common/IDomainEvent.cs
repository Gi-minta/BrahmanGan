namespace BrahmanGan.Domain.Common;

/// <summary>Interfaz para eventos de dominio.</summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    Guid EventId { get; }
}

/// <summary>Clase base abstracta (record) para eventos de dominio.</summary>
public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid EventId { get; init; } = Guid.NewGuid();
}
