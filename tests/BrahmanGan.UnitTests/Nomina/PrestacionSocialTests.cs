using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Nomina;
using Xunit;

namespace BrahmanGan.UnitTests.Nomina;

public class PrestacionSocialTests
{
    [Fact]
    public void Liquidar_valido_fija_periodo_y_salario()
    {
        var p = PrestacionSocial.Liquidar(TrabajadorId.New(), 2026, 6, salarioBase: 1_500_000m, cesantias: 125_000m);

        Assert.Equal(2026, p.Anio);
        Assert.Equal(6, p.Mes);
        Assert.Equal(1_500_000m, p.SalarioBase);
        Assert.Equal(125_000m, p.Cesantias);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Liquidar_con_mes_fuera_de_rango_lanza(int mes)
    {
        Assert.Throws<DomainException>(() =>
            PrestacionSocial.Liquidar(TrabajadorId.New(), 2026, mes, 1_000_000m));
    }

    [Fact]
    public void Liquidar_con_anio_invalido_lanza()
    {
        Assert.Throws<DomainException>(() =>
            PrestacionSocial.Liquidar(TrabajadorId.New(), 1899, 6, 1_000_000m));
    }

    [Fact]
    public void Liquidar_con_salario_base_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            PrestacionSocial.Liquidar(TrabajadorId.New(), 2026, 6, salarioBase: -1m));
    }
}
