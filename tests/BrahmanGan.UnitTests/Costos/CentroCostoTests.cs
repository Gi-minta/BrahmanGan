using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Costos;
using Xunit;

namespace BrahmanGan.UnitTests.Costos;

public class CentroCostoTests
{
    [Fact]
    public void Crear_valido_normaliza_codigo()
    {
        var c = CentroCosto.Crear("prod-1", "Producción lechera");

        Assert.Equal("PROD-1", c.Codigo);
        Assert.True(c.Activo);
    }

    [Theory]
    [InlineData("", "Nombre")]
    [InlineData("CC", "")]
    public void Crear_con_datos_vacios_lanza(string codigo, string nombre)
    {
        Assert.Throws<DomainException>(() => CentroCosto.Crear(codigo, nombre));
    }

    // ── Activo ─────────────────────────────────────────────────
    [Fact]
    public void Activo_crear_valido()
    {
        var a = Activo.Crear(CentroCostoId.New(), "Tractor", valorCompra: 80_000_000m, vidaUtilAnios: 10);
        Assert.Equal("Tractor", a.Descripcion);
        Assert.True(a.EstaActivo);
    }

    [Fact]
    public void Activo_sin_descripcion_lanza()
    {
        Assert.Throws<DomainException>(() => Activo.Crear(CentroCostoId.New(), ""));
    }

    [Fact]
    public void Activo_con_valores_negativos_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Activo.Crear(CentroCostoId.New(), "Tractor", valorCompra: -1m));
    }

    [Fact]
    public void Activo_con_vida_util_negativa_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Activo.Crear(CentroCostoId.New(), "Tractor", vidaUtilAnios: -1));
    }
}
