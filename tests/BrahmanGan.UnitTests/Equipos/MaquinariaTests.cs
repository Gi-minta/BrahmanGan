using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Equipos;
using Xunit;

namespace BrahmanGan.UnitTests.Equipos;

public class MaquinariaTests
{
    private static Maquinaria Crear() =>
        Maquinaria.Crear(CentroCostoId.New(), "MAQ-1", "Tractor John Deere", anio: 2020, valorCompra: 90_000_000m);

    [Fact]
    public void Crear_valido_queda_operativo_sin_horas()
    {
        var m = Crear();

        Assert.Equal(EstadoMaquinaria.OPERATIVO, m.Estado);
        Assert.Equal(0m, m.HorasUso);
    }

    [Theory]
    [InlineData("", "Nombre")]
    [InlineData("MAQ-1", "")]
    public void Crear_con_datos_vacios_lanza(string codigo, string nombre)
    {
        Assert.Throws<DomainException>(() => Maquinaria.Crear(CentroCostoId.New(), codigo, nombre));
    }

    [Fact]
    public void Crear_con_anio_invalido_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Maquinaria.Crear(CentroCostoId.New(), "MAQ-1", "Tractor", anio: 1800));
    }

    [Fact]
    public void RegistrarHoras_acumula_el_uso()
    {
        var m = Crear();

        m.RegistrarHoras(10m);
        m.RegistrarHoras(5.5m);

        Assert.Equal(15.5m, m.HorasUso);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void RegistrarHoras_no_positivas_lanza(int horas)
    {
        var m = Crear();

        Assert.Throws<BusinessRuleException>(() => m.RegistrarHoras(horas));
    }

    [Fact]
    public void CambiarEstado_desde_baja_lanza()
    {
        var m = Crear();
        m.CambiarEstado(EstadoMaquinaria.BAJA);

        Assert.Throws<BusinessRuleException>(() => m.CambiarEstado(EstadoMaquinaria.OPERATIVO));
    }

    [Fact]
    public void CambiarEstado_valido_cambia()
    {
        var m = Crear();

        m.CambiarEstado(EstadoMaquinaria.MANTENIMIENTO);

        Assert.Equal(EstadoMaquinaria.MANTENIMIENTO, m.Estado);
    }
}
