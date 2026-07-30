using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Finca;
using Xunit;

namespace BrahmanGan.UnitTests.Finca;

public class PotreroTests
{
    [Fact]
    public void Crear_valido_normaliza_codigo_a_mayusculas()
    {
        var p = Potrero.Crear(FincaId.New(), "p-01", "Potrero norte", areaHectareas: 8m);

        Assert.Equal("P-01", p.Codigo);
        Assert.True(p.Activo);
    }

    [Theory]
    [InlineData("", "Nombre")]
    [InlineData("P1", "")]
    public void Crear_con_datos_requeridos_vacios_lanza(string codigo, string nombre)
    {
        Assert.Throws<DomainException>(() => Potrero.Crear(FincaId.New(), codigo, nombre));
    }

    [Fact]
    public void Crear_con_area_negativa_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Potrero.Crear(FincaId.New(), "P1", "Norte", areaHectareas: -1m));
    }

    [Fact]
    public void Desactivar_y_activar_cambian_el_estado()
    {
        var p = Potrero.Crear(FincaId.New(), "P1", "Norte");

        p.Desactivar();
        Assert.False(p.Activo);

        p.Activar();
        Assert.True(p.Activo);
    }
}
