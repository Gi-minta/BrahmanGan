using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Sostenibilidad;
using Xunit;

namespace BrahmanGan.UnitTests.Sostenibilidad;

public class CapturaCarbonoTests
{
    [Fact]
    public void Registrar_valido_calcula_huella_neta()
    {
        var c = CapturaCarbono.Registrar(FincaId.New(), 2026, 6, emisiones: 120m, captura: 80m);

        Assert.Equal(40m, c.HuellaNeta); // 120 - 80
    }

    [Fact]
    public void HuellaNeta_trata_valores_nulos_como_cero()
    {
        var c = CapturaCarbono.Registrar(FincaId.New(), 2026, 6, emisiones: 50m);

        Assert.Equal(50m, c.HuellaNeta);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Registrar_con_mes_fuera_de_rango_lanza(int mes)
    {
        Assert.Throws<DomainException>(() =>
            CapturaCarbono.Registrar(FincaId.New(), 2026, mes));
    }

    [Fact]
    public void Registrar_con_anio_invalido_lanza()
    {
        Assert.Throws<DomainException>(() => CapturaCarbono.Registrar(FincaId.New(), 1899, 6));
    }

    [Fact]
    public void Registrar_con_valores_negativos_lanza()
    {
        Assert.Throws<DomainException>(() =>
            CapturaCarbono.Registrar(FincaId.New(), 2026, 6, emisiones: -1m));
    }
}
