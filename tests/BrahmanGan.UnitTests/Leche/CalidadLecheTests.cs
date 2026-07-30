using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Leche;
using Xunit;

namespace BrahmanGan.UnitTests.Leche;

public class CalidadLecheTests
{
    private static readonly DateOnly Hoy = new(2026, 5, 1);

    [Fact]
    public void Registrar_valida_fija_datos()
    {
        var c = CalidadLeche.Registrar(Hoy, AnimalId.New(), celSomaticas: 180_000,
            grasa: 3.6m, proteina: 3.2m, laboratorio: "LabX");

        Assert.Equal(180_000, c.CelSomaticas);
        Assert.Equal(3.6m, c.GrasaPct);
        Assert.Equal("LabX", c.Laboratorio);
    }

    [Fact]
    public void Registrar_solo_con_fecha_es_valido()
    {
        var c = CalidadLeche.Registrar(Hoy);

        Assert.Equal(Hoy, c.Fecha);
        Assert.Null(c.IdAnimal);
        Assert.Null(c.CelSomaticas);
    }

    [Fact]
    public void Registrar_celulas_somaticas_negativas_lanza()
    {
        Assert.Throws<DomainException>(() =>
            CalidadLeche.Registrar(Hoy, celSomaticas: -1));
    }
}
