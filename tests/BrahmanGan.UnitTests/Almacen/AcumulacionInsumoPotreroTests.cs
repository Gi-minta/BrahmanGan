using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Almacen;
using Xunit;

namespace BrahmanGan.UnitTests.Almacen;

public class AcumulacionInsumoPotreroTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    [Fact]
    public void Registrar_valido_fija_datos()
    {
        var a = AcumulacionInsumoPotrero.Registrar(PotreroId.New(), InsumoId.New(), Hoy,
            cantidad: 50m, costoUnitario: 3m);

        Assert.Equal(50m, a.Cantidad);
        Assert.Equal(3m, a.CostoUnitario);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Registrar_con_cantidad_no_positiva_lanza(int cantidad)
    {
        Assert.Throws<DomainException>(() =>
            AcumulacionInsumoPotrero.Registrar(PotreroId.New(), InsumoId.New(), Hoy, cantidad));
    }

    [Fact]
    public void Registrar_con_costo_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            AcumulacionInsumoPotrero.Registrar(PotreroId.New(), InsumoId.New(), Hoy, cantidad: 10m, costoUnitario: -1m));
    }
}
