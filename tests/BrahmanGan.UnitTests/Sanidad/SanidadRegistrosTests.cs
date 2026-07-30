using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Sanidad;
using Xunit;

namespace BrahmanGan.UnitTests.Sanidad;

/// <summary>Validaciones de creación de los registros sanitarios más simples.</summary>
public class SanidadRegistrosTests
{
    private static readonly DateOnly Hoy = new(2026, 6, 1);

    // ── ControlPreventivo ──────────────────────────────────────
    [Fact]
    public void ControlPreventivo_crear_valido()
    {
        var c = ControlPreventivo.Crear("Vacunación aftosa", periodicidad: "Semestral");
        Assert.Equal("Vacunación aftosa", c.Nombre);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ControlPreventivo_sin_nombre_lanza(string nombre)
    {
        Assert.Throws<DomainException>(() => ControlPreventivo.Crear(nombre));
    }

    // ── HistorialDesparasitacion ───────────────────────────────
    [Fact]
    public void Desparasitacion_aplicar_valido()
    {
        var d = HistorialDesparasitacion.Aplicar(AnimalId.New(), MedicamentoId.New(), Hoy, dosis: 4m, tipoParasito: "Interno");
        Assert.Equal(4m, d.Dosis);
    }

    [Fact]
    public void Desparasitacion_dosis_negativa_lanza()
    {
        Assert.Throws<DomainException>(() =>
            HistorialDesparasitacion.Aplicar(AnimalId.New(), MedicamentoId.New(), Hoy, dosis: -1m));
    }

    // ── HistorialMastitis ──────────────────────────────────────
    [Fact]
    public void Mastitis_registrar_valido()
    {
        var m = HistorialMastitis.Registrar(AnimalId.New(), Hoy, cuarto: "AI", grado: "Leve");
        Assert.Equal("AI", m.Cuarto);
    }

    // ── Complemento ────────────────────────────────────────────
    [Fact]
    public void Complemento_registrar_valido()
    {
        var c = Complemento.Registrar(HistorialCurativoId.New(), Hoy, "Fluidoterapia", costo: 12m);
        Assert.Equal("Fluidoterapia", c.Descripcion);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Complemento_sin_descripcion_lanza(string descripcion)
    {
        Assert.Throws<DomainException>(() =>
            Complemento.Registrar(HistorialCurativoId.New(), Hoy, descripcion));
    }

    [Fact]
    public void Complemento_costo_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Complemento.Registrar(HistorialCurativoId.New(), Hoy, "X", costo: -1m));
    }
}
