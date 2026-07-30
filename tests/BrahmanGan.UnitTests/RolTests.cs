using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Seguridad;
using Xunit;

namespace BrahmanGan.UnitTests;

public class RolTests
{
    [Fact]
    public void Crear_valido_queda_activo()
    {
        var rol = Rol.Crear("Operador", "Registro diario");

        Assert.Equal("Operador", rol.Nombre);
        Assert.True(rol.Activo);
        Assert.False(rol.EsSistema);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Crear_con_nombre_vacio_lanza(string nombre)
    {
        Assert.Throws<BusinessRuleException>(() => Rol.Crear(nombre, "desc"));
    }

    [Fact]
    public void Actualizar_rol_del_sistema_lanza()
    {
        var rol = Rol.Crear("Administrador", "Acceso total", esSistema: true);

        Assert.Throws<BusinessRuleException>(() => rol.Actualizar("Otro", "x"));
    }

    [Fact]
    public void Desactivar_rol_del_sistema_lanza()
    {
        var rol = Rol.Crear("Administrador", "Acceso total", esSistema: true);

        Assert.Throws<BusinessRuleException>(() => rol.Desactivar());
    }

    [Fact]
    public void Actualizar_rol_normal_cambia_datos()
    {
        var rol = Rol.Crear("Operador", "vieja");

        rol.Actualizar("  Operador Senior  ", "nueva");

        Assert.Equal("Operador Senior", rol.Nombre);
        Assert.Equal("nueva", rol.Descripcion);
    }

    [Fact]
    public void Desactivar_rol_normal_lo_inactiva()
    {
        var rol = Rol.Crear("Operador", "desc");

        rol.Desactivar();

        Assert.False(rol.Activo);
    }
}
