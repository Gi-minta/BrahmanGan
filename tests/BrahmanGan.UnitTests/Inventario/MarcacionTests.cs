using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Inventario;
using Xunit;

namespace BrahmanGan.UnitTests.Inventario;

public class MarcacionTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    private static Marcacion Aplicar() =>
        Marcacion.Aplicar(AnimalId.New(), TipoMarcacion.ARETE, "A-123", Hoy);

    [Fact]
    public void Aplicar_valida_queda_activa()
    {
        var m = Aplicar();

        Assert.True(m.Activa);
        Assert.Equal("A-123", m.Codigo);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Aplicar_sin_codigo_lanza(string codigo)
    {
        Assert.Throws<DomainException>(() =>
            Marcacion.Aplicar(AnimalId.New(), TipoMarcacion.ARETE, codigo, Hoy));
    }

    [Fact]
    public void DarDeBaja_desactiva_y_registra_motivo()
    {
        var m = Aplicar();

        m.DarDeBaja(Hoy.AddDays(100), "Pérdida del arete");

        Assert.False(m.Activa);
        Assert.Equal(Hoy.AddDays(100), m.FechaBaja);
        Assert.Equal("Pérdida del arete", m.MotivoBaja);
    }

    [Fact]
    public void DarDeBaja_dos_veces_lanza()
    {
        var m = Aplicar();
        m.DarDeBaja(Hoy.AddDays(100), "Pérdida");

        Assert.Throws<BusinessRuleException>(() => m.DarDeBaja(Hoy.AddDays(120), "Otro"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void DarDeBaja_sin_motivo_lanza(string motivo)
    {
        var m = Aplicar();

        Assert.Throws<DomainException>(() => m.DarDeBaja(Hoy.AddDays(100), motivo));
    }
}
