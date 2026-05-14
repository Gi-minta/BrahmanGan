using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Costos;

/// <summary>Ingreso financiero asociado a un centro de costo.</summary>
public sealed class Ingreso : Entity<IngresoId>
{
    public DateOnly Fecha { get; private set; }
    public CentroCostoId IdCentro { get; private set; } = null!;
    public string TipoIngreso { get; private set; } = string.Empty;
    public string? Concepto { get; private set; }
    public decimal Valor { get; private set; }
    public string? Comprobante { get; private set; }

    private Ingreso() { }
    public static Ingreso Crear(DateOnly fecha, CentroCostoId idCentro, string tipoIngreso, decimal valor,
        string? concepto = null, string? comprobante = null)
    {
        if (string.IsNullOrWhiteSpace(tipoIngreso)) throw new DomainException("Tipo de ingreso requerido");
        if (valor <= 0) throw new DomainException("Valor debe ser > 0");
        return new Ingreso { Id = IngresoId.New(), Fecha = fecha, IdCentro = idCentro, TipoIngreso = tipoIngreso.Trim(),
            Valor = valor, Concepto = concepto, Comprobante = comprobante };
    }
}
