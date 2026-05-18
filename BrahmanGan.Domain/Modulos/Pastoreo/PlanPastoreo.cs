using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Pastoreo;

/// <summary>Plan de pastoreo rotacional para un potrero.</summary>
public sealed class PlanPastoreo : AggregateRoot<PlanPastoreoId>
{
    public PotreroId IdPotrero { get; private set; } = null!;
    public DateOnly FechaInicio { get; private set; }
    public DateOnly? FechaFin { get; private set; }
    public int? NumAnimales { get; private set; }
    public decimal? CapacidadCarga { get; private set; }
    public string? Observaciones { get; private set; }
    public bool Activo { get; private set; } = true;

    private PlanPastoreo() { }

    public static PlanPastoreo Crear(PotreroId idPotrero, DateOnly fechaInicio,
        DateOnly? fechaFin = null, int? numAnimales = null,
        decimal? capacidadCarga = null, string? observaciones = null)
    {
        if (fechaFin.HasValue && fechaFin.Value < fechaInicio)
            throw new DomainException("Fecha fin no puede ser anterior a fecha inicio");
        if (numAnimales is < 0) throw new DomainException("Número de animales no puede ser negativo");
        if (capacidadCarga is < 0) throw new DomainException("Capacidad de carga no puede ser negativa");
        return new PlanPastoreo
        {
            Id = PlanPastoreoId.New(),
            IdPotrero = idPotrero,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            NumAnimales = numAnimales,
            CapacidadCarga = capacidadCarga,
            Observaciones = observaciones,
            Activo = true
        };
    }

    public void Finalizar(DateOnly fechaFin)
    {
        if (fechaFin < FechaInicio) throw new BusinessRuleException("Fecha fin no puede ser anterior a fecha inicio");
        FechaFin = fechaFin;
        Activo = false;
        IncrementVersion();
    }
}
