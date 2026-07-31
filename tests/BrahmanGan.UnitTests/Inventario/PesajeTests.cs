using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Inventario;
using Xunit;

namespace BrahmanGan.UnitTests.Inventario;

public class PesajeTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    [Fact]
    public void Registrar_valido_fija_peso()
    {
        var p = Pesaje.Registrar(AnimalId.New(), Hoy, pesoKg: 380m, condicionCorporal: 5m);

        Assert.Equal(380m, p.PesoKg);
        Assert.Equal(5m, p.CondicionCorporal);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Registrar_con_peso_no_positivo_lanza(int peso)
    {
        Assert.Throws<DomainException>(() => Pesaje.Registrar(AnimalId.New(), Hoy, peso));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(9)]
    public void Registrar_acepta_condicion_corporal_en_los_limites(int cc)
    {
        var p = Pesaje.Registrar(AnimalId.New(), Hoy, 380m, condicionCorporal: cc);

        Assert.Equal(cc, p.CondicionCorporal);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public void Registrar_con_condicion_corporal_fuera_de_rango_lanza(int cc)
    {
        Assert.Throws<DomainException>(() =>
            Pesaje.Registrar(AnimalId.New(), Hoy, 380m, condicionCorporal: cc));
    }
}
