using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Alimentacion;

/// <summary>Plan de alimentación para animales de una finca.</summary>
public sealed class PlanAlimentacion : AggregateRoot<PlanAlimentacionId>
{
    public FincaId IdFinca { get; private set; } = null!;
    public string Nombre { get; private set; } = string.Empty;
    public DateOnly FechaInicio { get; private set; }
    public DateOnly? FechaFin { get; private set; }
    public string? Observaciones { get; private set; }
    public bool Activo { get; private set; } = true;

    private PlanAlimentacion() { }

    public static PlanAlimentacion Crear(FincaId idFinca, string nombre, DateOnly fechaInicio,
        DateOnly? fechaFin = null, string? observaciones = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre del plan requerido");
        if (fechaFin.HasValue && fechaFin.Value < fechaInicio)
            throw new DomainException("Fecha fin no puede ser anterior a fecha inicio");
        return new PlanAlimentacion
        {
            Id = PlanAlimentacionId.New(),
            IdFinca = idFinca,
            Nombre = nombre.Trim(),
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            Observaciones = observaciones,
            Activo = true
        };
    }

    public void Desactivar()
    {
        Activo = false;
        IncrementVersion();
    }
}
