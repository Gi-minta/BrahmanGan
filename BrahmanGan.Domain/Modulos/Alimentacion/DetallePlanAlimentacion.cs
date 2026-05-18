using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Alimentacion;

/// <summary>Línea de detalle de un plan de alimentación: qué alimento, cuánto y con qué frecuencia.</summary>
public sealed class DetallePlanAlimentacion : Entity<DetallePlanAlimentacionId>
{
    public PlanAlimentacionId IdPlan { get; private set; } = null!;
    public InsumoId? IdInsumo { get; private set; }
    public string Alimento { get; private set; } = string.Empty;
    public decimal CantidadDiaria { get; private set; }
    public string? UnidadMedida { get; private set; }
    public string? Observaciones { get; private set; }

    private DetallePlanAlimentacion() { }

    public static DetallePlanAlimentacion Crear(PlanAlimentacionId idPlan, string alimento,
        decimal cantidadDiaria, string? unidadMedida = null,
        InsumoId? idInsumo = null, string? observaciones = null)
    {
        if (string.IsNullOrWhiteSpace(alimento)) throw new DomainException("Nombre del alimento requerido");
        if (cantidadDiaria <= 0) throw new DomainException("Cantidad diaria debe ser mayor a cero");
        return new DetallePlanAlimentacion
        {
            Id = DetallePlanAlimentacionId.New(),
            IdPlan = idPlan,
            IdInsumo = idInsumo,
            Alimento = alimento.Trim(),
            CantidadDiaria = cantidadDiaria,
            UnidadMedida = unidadMedida,
            Observaciones = observaciones
        };
    }
}
