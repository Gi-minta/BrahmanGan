using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.ValueObjects;
using Xunit;

namespace BrahmanGan.UnitTests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_valido_normaliza_a_minusculas()
    {
        var email = Email.Create("Admin@Test.COM");

        Assert.Equal("admin@test.com", email.Address);
    }

    [Theory]
    [InlineData(" user@test.com")]
    [InlineData("user@test.com ")]
    public void Create_con_espacios_alrededor_lanza(string address)
    {
        // El formato se valida antes de recortar, por lo que los espacios lo invalidan.
        Assert.Throws<DomainException>(() => Email.Create(address));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_vacio_lanza(string address)
    {
        Assert.Throws<DomainException>(() => Email.Create(address));
    }

    [Theory]
    [InlineData("sinarroba")]
    [InlineData("a@b")]
    [InlineData("a@b@c.com")]
    [InlineData("con espacio@b.com")]
    public void Create_formato_invalido_lanza(string address)
    {
        Assert.Throws<DomainException>(() => Email.Create(address));
    }

    [Fact]
    public void Dos_emails_con_la_misma_direccion_son_iguales()
    {
        var a = Email.Create("user@Test.com");
        var b = Email.Create("USER@test.com");

        Assert.Equal(a, b);
    }

    [Fact]
    public void Conversion_implicita_a_string_devuelve_la_direccion()
    {
        var email = Email.Create("user@test.com");
        string address = email;

        Assert.Equal("user@test.com", address);
    }
}
