using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Leche;
using Xunit;

namespace BrahmanGan.UnitTests.Leche;

public class ControlLecheAnimalTests
{
    private static readonly DateOnly Hoy = new(2026, 5, 1);

    [Fact]
    public void Registrar_suma_los_tres_ordenos_en_total()
    {
        var c = ControlLecheAnimal.Registrar(AnimalId.New(), Hoy, maniana: 5m, tarde: 4m, noche: 3m);

        Assert.Equal(12m, c.TotalLitros);
    }

    [Fact]
    public void Registrar_trata_ordenos_nulos_como_cero()
    {
        var c = ControlLecheAnimal.Registrar(AnimalId.New(), Hoy, maniana: 6m);

        Assert.Equal(6m, c.TotalLitros);
    }

    [Fact]
    public void Registrar_con_litros_negativos_lanza()
    {
        Assert.Throws<DomainException>(() =>
            ControlLecheAnimal.Registrar(AnimalId.New(), Hoy, maniana: -1m, tarde: 5m));
    }

    [Fact]
    public void Registrar_sin_ningun_litro_lanza()
    {
        Assert.Throws<DomainException>(() =>
            ControlLecheAnimal.Registrar(AnimalId.New(), Hoy));
    }

    [Fact]
    public void Registrar_con_total_cero_lanza()
    {
        Assert.Throws<DomainException>(() =>
            ControlLecheAnimal.Registrar(AnimalId.New(), Hoy, maniana: 0m, tarde: 0m, noche: 0m));
    }
}
