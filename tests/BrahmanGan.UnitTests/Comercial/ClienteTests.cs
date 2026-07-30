using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Comercial;
using Xunit;

namespace BrahmanGan.UnitTests.Comercial;

public class ClienteTests
{
    [Fact]
    public void Crear_valido_queda_activo_y_emite_evento()
    {
        var c = Cliente.Crear("900123", "Lácteos SAS");

        Assert.True(c.Activo);
        Assert.Equal("Lácteos SAS", c.RazonSocial);
        Assert.Contains(c.DomainEvents, e => e.GetType().Name == "ClienteCreadoEvent");
    }

    [Fact]
    public void Crear_normaliza_el_tipo_de_documento_a_mayusculas()
    {
        var c = Cliente.Crear("900123", "Lácteos SAS", tipoDocumento: "nit");

        Assert.Equal("NIT", c.TipoDocumento);
    }

    [Theory]
    [InlineData("", "Razón")]
    [InlineData("900123", "")]
    public void Crear_con_datos_requeridos_vacios_lanza(string documento, string razon)
    {
        Assert.Throws<DomainException>(() => Cliente.Crear(documento, razon));
    }

    [Fact]
    public void Desactivar_y_activar_cambian_el_estado()
    {
        var c = Cliente.Crear("900123", "Lácteos SAS");

        c.Desactivar();
        Assert.False(c.Activo);

        c.Activar();
        Assert.True(c.Activo);
    }
}
