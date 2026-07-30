using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Reproduccion;
using Xunit;

namespace BrahmanGan.UnitTests.Reproduccion;

public class NacimientoTests
{
    private static readonly DateOnly Fecha = new(2026, 7, 1);

    [Theory]
    [InlineData('M')]
    [InlineData('H')]
    public void Registrar_valido_acepta_sexo_M_o_H(char sexo)
    {
        var n = Nacimiento.Registrar(GestacionId.New(), Fecha, sexo: sexo, pesoNacimiento: 32m);

        Assert.Equal(sexo, n.Sexo);
        Assert.Equal(32m, n.PesoNacimiento);
    }

    [Fact]
    public void Registrar_con_sexo_invalido_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Nacimiento.Registrar(GestacionId.New(), Fecha, sexo: 'X'));
    }

    [Fact]
    public void Registrar_con_peso_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Nacimiento.Registrar(GestacionId.New(), Fecha, pesoNacimiento: -1m));
    }

    [Fact]
    public void VincularCria_asigna_el_animal()
    {
        var n = Nacimiento.Registrar(GestacionId.New(), Fecha, sexo: 'H');

        n.VincularCria(AnimalId.From(42));

        Assert.Equal(42, n.IdAnimalCria!.Value);
    }
}
