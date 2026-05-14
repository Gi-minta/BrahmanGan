using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.Modulos.Almacen;

/// <summary>Movimiento de Kardex (auditoría de inventario).</summary>
public sealed class KardexInsumo : Entity<KardexInsumoId>
{
    public InsumoId IdInsumo { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public TipoMovimientoKardex TipoMovimiento { get; private set; }
    public decimal Cantidad { get; private set; }
    public decimal? CostoUnitario { get; private set; }
    public string? Concepto { get; private set; }
    public string? Referencia { get; private set; }
    public decimal SaldoAnterior { get; private set; }
    public decimal SaldoNuevo { get; private set; }

    private KardexInsumo() { }
    public static KardexInsumo Registrar(InsumoId idInsumo, DateOnly fecha, TipoMovimientoKardex tipo,
        decimal cantidad, decimal saldoAnterior, decimal saldoNuevo,
        decimal? costoUnitario = null, string? concepto = null, string? referencia = null)
    {
        return new KardexInsumo { Id = KardexInsumoId.New(), IdInsumo = idInsumo, Fecha = fecha,
            TipoMovimiento = tipo, Cantidad = cantidad, CostoUnitario = costoUnitario,
            Concepto = concepto, Referencia = referencia,
            SaldoAnterior = saldoAnterior, SaldoNuevo = saldoNuevo };
    }
}
