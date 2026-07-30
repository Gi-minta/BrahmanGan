using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Almacen;
using Xunit;

namespace BrahmanGan.UnitTests;

public class InsumoTests
{
    [Fact]
    public void Crear_valido_inicializa_stock()
    {
        var insumo = Insumo.Crear("INS-1", "Sal mineral", stockMinimo: 10, stockInicial: 50);

        Assert.Equal(50, insumo.StockActual);
        Assert.True(insumo.Activo);
    }

    [Theory]
    [InlineData("", "Nombre")]
    [InlineData("COD", "")]
    public void Crear_con_datos_requeridos_vacios_lanza(string codigo, string nombre)
    {
        Assert.Throws<DomainException>(() => Insumo.Crear(codigo, nombre));
    }

    [Fact]
    public void AplicarMovimiento_entrada_suma_stock()
    {
        var insumo = Insumo.Crear("INS-1", "Sal", stockInicial: 20);

        var (anterior, nuevo) = insumo.AplicarMovimiento(TipoMovimientoKardex.ENTRADA, 5);

        Assert.Equal(20, anterior);
        Assert.Equal(25, nuevo);
        Assert.Equal(25, insumo.StockActual);
    }

    [Fact]
    public void AplicarMovimiento_salida_resta_stock()
    {
        var insumo = Insumo.Crear("INS-1", "Sal", stockInicial: 20);

        var (_, nuevo) = insumo.AplicarMovimiento(TipoMovimientoKardex.SALIDA, 8);

        Assert.Equal(12, nuevo);
    }

    [Fact]
    public void AplicarMovimiento_ajuste_fija_stock()
    {
        var insumo = Insumo.Crear("INS-1", "Sal", stockInicial: 20);

        var (_, nuevo) = insumo.AplicarMovimiento(TipoMovimientoKardex.AJUSTE, 7);

        Assert.Equal(7, nuevo);
    }

    [Fact]
    public void AplicarMovimiento_salida_mayor_al_stock_lanza()
    {
        var insumo = Insumo.Crear("INS-1", "Sal", stockInicial: 5);

        Assert.Throws<BusinessRuleException>(() =>
            insumo.AplicarMovimiento(TipoMovimientoKardex.SALIDA, 6));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void AplicarMovimiento_cantidad_no_positiva_lanza(int cantidad)
    {
        var insumo = Insumo.Crear("INS-1", "Sal", stockInicial: 5);

        Assert.Throws<BusinessRuleException>(() =>
            insumo.AplicarMovimiento(TipoMovimientoKardex.ENTRADA, cantidad));
    }

    [Theory]
    [InlineData(4, 10, true)]
    [InlineData(10, 10, false)]
    [InlineData(15, 10, false)]
    public void BajoMinimo_refleja_stock_vs_minimo(int stock, int minimo, bool esperado)
    {
        var insumo = Insumo.Crear("INS-1", "Sal", stockMinimo: minimo, stockInicial: stock);

        Assert.Equal(esperado, insumo.BajoMinimo());
    }
}
