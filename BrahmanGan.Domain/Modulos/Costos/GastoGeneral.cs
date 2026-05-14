using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Costos;

/// <summary>Gasto general (no asociado necesariamente a un centro fijo).</summary>
public sealed class GastoGeneral : Entity<GastoGeneralId>
{
    public DateOnly Fecha { get; private set; }
    public CentroCostoId? IdCentro { get; private set; }
    public string Concepto { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
    public string? Proveedor { get; private set; }
    public string? Comprobante { get; private set; }

    private GastoGeneral() { }
    public static GastoGeneral Crear(DateOnly fecha, string concepto, decimal valor,
        CentroCostoId? idCentro = null, string? proveedor = null, string? comprobante = null)
    {
        if (string.IsNullOrWhiteSpace(concepto)) throw new DomainException("Concepto requerido");
        if (valor < 0) throw new DomainException("Valor no negativo");
        return new GastoGeneral { Id = GastoGeneralId.New(), Fecha = fecha, IdCentro = idCentro, Concepto = concepto.Trim(),
            Valor = valor, Proveedor = proveedor, Comprobante = comprobante };
    }
}
