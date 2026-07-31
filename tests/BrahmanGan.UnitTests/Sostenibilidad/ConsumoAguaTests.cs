using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Sostenibilidad;
using Xunit;

namespace BrahmanGan.UnitTests.Sostenibilidad;

public class ConsumoAguaTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    [Fact]
    public void Registrar_calcula_litros_por_animal_dia()
    {
        var c = ConsumoAgua.Registrar(FincaId.New(), Hoy, volumenM3: 10m, numAnimales: 100);

        // (10 m3 * 1000) / 100 = 100 litros/animal
        Assert.Equal(100m, c.LitrosAnimalDia);
    }

    [Fact]
    public void LitrosAnimalDia_es_null_sin_numero_de_animales()
    {
        var c = ConsumoAgua.Registrar(FincaId.New(), Hoy, volumenM3: 10m);

        Assert.Null(c.LitrosAnimalDia);
    }

    [Fact]
    public void Registrar_con_volumen_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            ConsumoAgua.Registrar(FincaId.New(), Hoy, volumenM3: -1m));
    }

    [Fact]
    public void Registrar_con_numero_de_animales_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            ConsumoAgua.Registrar(FincaId.New(), Hoy, volumenM3: 5m, numAnimales: -1));
    }
}
