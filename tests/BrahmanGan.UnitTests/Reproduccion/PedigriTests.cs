using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Reproduccion;
using Xunit;

namespace BrahmanGan.UnitTests.Reproduccion;

public class PedigriTests
{
    [Fact]
    public void Crear_valido_fija_datos()
    {
        var p = Pedigri.Crear(AnimalId.New(), abuelo1: AnimalId.From(10), puntajeMorfologia: 87.5m);

        Assert.Equal(10, p.IdAbuelo1!.Value);
        Assert.Equal(87.5m, p.PuntajeMorfologia);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    public void Crear_acepta_puntaje_en_los_limites(int puntaje)
    {
        var p = Pedigri.Crear(AnimalId.New(), puntajeMorfologia: puntaje);

        Assert.Equal(puntaje, p.PuntajeMorfologia);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Crear_con_puntaje_fuera_de_rango_lanza(int puntaje)
    {
        Assert.Throws<DomainException>(() =>
            Pedigri.Crear(AnimalId.New(), puntajeMorfologia: puntaje));
    }
}
