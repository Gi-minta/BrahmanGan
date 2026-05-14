using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>Detalle (medicamento + dosis + costo) de un tratamiento curativo.</summary>
public sealed class DetalleCurativo : Entity<DetalleCurativoId>
{
    public HistorialCurativoId IdTratamiento { get; private set; } = null!;
    public MedicamentoId IdMedicamento { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public decimal? Dosis { get; private set; }
    public decimal? CostoUnitario { get; private set; }

    private DetalleCurativo() { }

    internal static DetalleCurativo Crear(HistorialCurativoId idTrat, MedicamentoId idMed, DateOnly fecha, decimal? dosis, decimal? costoUnit)
    {
        return new DetalleCurativo
        {
            Id = DetalleCurativoId.New(),
            IdTratamiento = idTrat,
            IdMedicamento = idMed,
            Fecha = fecha,
            Dosis = dosis,
            CostoUnitario = costoUnit
        };
    }
}
