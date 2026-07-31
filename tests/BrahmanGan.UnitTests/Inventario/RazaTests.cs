using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Inventario;
using Xunit;

namespace BrahmanGan.UnitTests.Inventario;

public class RazaTests
{
    [Fact]
    public void Crear_valido_normaliza_codigo()
    {
        var r = Raza.Crear("brahman", "Brahman", PropositoRaza.CARNE);

        Assert.Equal("BRAHMAN", r.Codigo);
        Assert.Equal(PropositoRaza.CARNE, r.Proposito);
    }

    [Theory]
    [InlineData("", "Brahman")]
    [InlineData("BR", "")]
    public void Crear_con_datos_vacios_lanza(string codigo, string nombre)
    {
        Assert.Throws<DomainException>(() => Raza.Crear(codigo, nombre));
    }

    [Fact]
    public void Renombrar_cambia_el_nombre()
    {
        var r = Raza.Crear("BR", "Brahman");

        r.Renombrar("  Brahman Rojo  ");

        Assert.Equal("Brahman Rojo", r.Nombre);
    }

    [Fact]
    public void Renombrar_con_nombre_vacio_lanza()
    {
        var r = Raza.Crear("BR", "Brahman");

        Assert.Throws<DomainException>(() => r.Renombrar(""));
    }
}
