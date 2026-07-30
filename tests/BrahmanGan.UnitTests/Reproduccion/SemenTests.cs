using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Reproduccion;
using Xunit;

namespace BrahmanGan.UnitTests.Reproduccion;

public class SemenTests
{
    [Fact]
    public void Crear_valido_queda_activo_con_stock()
    {
        var s = Semen.Crear("SEM-1", "Toro Campeón", stockInicial: 10);

        Assert.Equal(10, s.StockDosis);
        Assert.True(s.Activo);
    }

    [Theory]
    [InlineData("", "Toro")]
    [InlineData("SEM-1", "")]
    public void Crear_con_datos_requeridos_vacios_lanza(string codigo, string toro)
    {
        Assert.Throws<DomainException>(() => Semen.Crear(codigo, toro));
    }

    [Fact]
    public void Crear_con_stock_inicial_negativo_lanza()
    {
        Assert.Throws<DomainException>(() => Semen.Crear("SEM-1", "Toro", stockInicial: -1));
    }

    [Fact]
    public void IngresarStock_incrementa_las_dosis()
    {
        var s = Semen.Crear("SEM-1", "Toro", stockInicial: 5);

        s.IngresarStock(3);

        Assert.Equal(8, s.StockDosis);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void IngresarStock_con_cantidad_no_positiva_lanza(int dosis)
    {
        var s = Semen.Crear("SEM-1", "Toro", stockInicial: 5);

        Assert.Throws<BusinessRuleException>(() => s.IngresarStock(dosis));
    }

    [Fact]
    public void ConsumirDosis_descuenta_del_stock()
    {
        var s = Semen.Crear("SEM-1", "Toro", stockInicial: 5);

        s.ConsumirDosis();      // por defecto 1
        s.ConsumirDosis(2);

        Assert.Equal(2, s.StockDosis);
    }

    [Fact]
    public void ConsumirDosis_con_stock_insuficiente_lanza()
    {
        var s = Semen.Crear("SEM-1", "Toro", stockInicial: 1);

        Assert.Throws<BusinessRuleException>(() => s.ConsumirDosis(2));
    }

    [Fact]
    public void ConsumirDosis_no_positiva_lanza()
    {
        var s = Semen.Crear("SEM-1", "Toro", stockInicial: 5);

        Assert.Throws<BusinessRuleException>(() => s.ConsumirDosis(0));
    }
}
