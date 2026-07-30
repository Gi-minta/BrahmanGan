using BrahmanGan.Domain.Modulos.Seguridad;
using Xunit;

namespace BrahmanGan.UnitTests;

public class UsuarioTests
{
    // Regresión: CrearLocal/CrearOAuth deben inicializar el Id (era el bug que hacía
    // fallar el seeding del admin con "primary key property 'Id' is null").
    [Fact]
    public void CrearLocal_inicializa_el_Id_como_transitorio()
    {
        var usuario = Usuario.CrearLocal("admin@brahmangan.com", "Admin", "hash");

        Assert.NotNull(usuario.Id);
        Assert.Equal(0, usuario.Id.Value);
        Assert.True(usuario.Id.IsTransient());
    }

    [Fact]
    public void CrearOAuth_inicializa_el_Id_como_transitorio()
    {
        var usuario = Usuario.CrearOAuth("user@gmail.com", "User", ProveedorAuth.Google, "ext-123");

        Assert.NotNull(usuario.Id);
        Assert.True(usuario.Id.IsTransient());
    }

    [Fact]
    public void CrearLocal_normaliza_el_email_a_minusculas()
    {
        var usuario = Usuario.CrearLocal("Admin@BrahmanGan.com", "Admin", "hash");

        Assert.Equal("admin@brahmangan.com", usuario.Email);
    }

    [Theory]
    [InlineData("sin-arroba")]
    [InlineData("")]
    public void CrearLocal_con_email_invalido_lanza(string email)
    {
        Assert.ThrowsAny<System.Exception>(() => Usuario.CrearLocal(email, "Admin", "hash"));
    }
}
