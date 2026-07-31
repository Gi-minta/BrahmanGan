using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Inventario;
using Xunit;

namespace BrahmanGan.UnitTests.Inventario;

/// <summary>Tests de Origen, MovimientoAnimal e HistorialAnimal.</summary>
public class InventarioRegistrosTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    // ── Origen ─────────────────────────────────────────────────
    [Fact]
    public void Origen_crear_normaliza_codigo()
    {
        var o = Origen.Crear("comprado", "Animal comprado");
        Assert.Equal("COMPRADO", o.Codigo);
    }

    [Theory]
    [InlineData("", "Desc")]
    [InlineData("COD", "")]
    public void Origen_con_datos_vacios_lanza(string codigo, string descripcion)
    {
        Assert.Throws<DomainException>(() => Origen.Crear(codigo, descripcion));
    }

    // ── MovimientoAnimal ───────────────────────────────────────
    [Fact]
    public void MovimientoAnimal_crear_valido()
    {
        var m = MovimientoAnimal.Crear(AnimalId.New(), TipoMovimientoAnimal.INGRESO, Hoy, valor: 2_000_000m, pesoKg: 300m);
        Assert.Equal(TipoMovimientoAnimal.INGRESO, m.TipoMovimiento);
        Assert.Equal(300m, m.PesoKg);
    }

    [Fact]
    public void MovimientoAnimal_con_peso_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            MovimientoAnimal.Crear(AnimalId.New(), TipoMovimientoAnimal.INGRESO, Hoy, pesoKg: -1m));
    }

    // ── HistorialAnimal ────────────────────────────────────────
    [Fact]
    public void HistorialAnimal_crear_valido()
    {
        var h = HistorialAnimal.Crear(AnimalId.New(), TipoEventoHistorial.COMPRA, Hoy, descripcion: "Compra inicial");
        Assert.Equal(TipoEventoHistorial.COMPRA, h.TipoEvento);
        Assert.Equal("Compra inicial", h.Descripcion);
    }
}
