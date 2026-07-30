using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Almacen;
using Xunit;

namespace BrahmanGan.UnitTests.Almacen;

public class KardexInsumoTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    [Fact]
    public void Registrar_captura_tipo_cantidad_y_saldos()
    {
        var k = KardexInsumo.Registrar(InsumoId.New(), Hoy, TipoMovimientoKardex.ENTRADA,
            cantidad: 20m, saldoAnterior: 100m, saldoNuevo: 120m, costoUnitario: 5m, concepto: "Compra");

        Assert.Equal(TipoMovimientoKardex.ENTRADA, k.TipoMovimiento);
        Assert.Equal(20m, k.Cantidad);
        Assert.Equal(100m, k.SaldoAnterior);
        Assert.Equal(120m, k.SaldoNuevo);
        Assert.Equal("Compra", k.Concepto);
    }

    [Fact]
    public void Registrar_salida_conserva_los_saldos_indicados()
    {
        var k = KardexInsumo.Registrar(InsumoId.New(), Hoy, TipoMovimientoKardex.SALIDA,
            cantidad: 8m, saldoAnterior: 120m, saldoNuevo: 112m);

        Assert.Equal(TipoMovimientoKardex.SALIDA, k.TipoMovimiento);
        Assert.Equal(120m, k.SaldoAnterior);
        Assert.Equal(112m, k.SaldoNuevo);
    }
}
