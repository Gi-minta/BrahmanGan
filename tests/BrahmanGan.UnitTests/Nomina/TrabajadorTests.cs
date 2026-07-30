using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Nomina;
using Xunit;

namespace BrahmanGan.UnitTests.Nomina;

public class TrabajadorTests
{
    private static readonly DateOnly Ingreso = new(2026, 1, 15);

    private static Trabajador Contratar() =>
        Trabajador.Contratar("123", "Ana", "Pérez", Ingreso, salarioBase: 1_500_000m);

    [Fact]
    public void Contratar_valido_queda_activo()
    {
        var t = Contratar();

        Assert.True(t.Activo);
        Assert.Null(t.FechaRetiro);
        Assert.Equal("Ana", t.Nombres);
    }

    [Theory]
    [InlineData("", "Ana", "Pérez")]
    [InlineData("123", "", "Pérez")]
    [InlineData("123", "Ana", "")]
    public void Contratar_con_datos_requeridos_vacios_lanza(string cedula, string nombres, string apellidos)
    {
        Assert.Throws<DomainException>(() => Trabajador.Contratar(cedula, nombres, apellidos, Ingreso));
    }

    [Fact]
    public void Contratar_con_salario_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Trabajador.Contratar("123", "Ana", "Pérez", Ingreso, salarioBase: -1m));
    }

    [Fact]
    public void Retirar_fija_fecha_y_desactiva()
    {
        var t = Contratar();

        t.Retirar(Ingreso.AddDays(200));

        Assert.False(t.Activo);
        Assert.Equal(Ingreso.AddDays(200), t.FechaRetiro);
    }

    [Fact]
    public void Retirar_dos_veces_lanza()
    {
        var t = Contratar();
        t.Retirar(Ingreso.AddDays(200));

        Assert.Throws<BusinessRuleException>(() => t.Retirar(Ingreso.AddDays(300)));
    }

    [Fact]
    public void Retirar_con_fecha_anterior_al_ingreso_lanza()
    {
        var t = Contratar();

        Assert.Throws<BusinessRuleException>(() => t.Retirar(Ingreso.AddDays(-1)));
    }
}
