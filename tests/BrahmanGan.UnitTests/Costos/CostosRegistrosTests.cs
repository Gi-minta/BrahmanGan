using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Costos;
using Xunit;

namespace BrahmanGan.UnitTests.Costos;

/// <summary>Validaciones de creación de los registros de costos e ingresos.</summary>
public class CostosRegistrosTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    // ── GastoGeneral ───────────────────────────────────────────
    [Fact]
    public void GastoGeneral_crear_valido()
    {
        var g = GastoGeneral.Crear(Hoy, "Combustible", 250_000m);
        Assert.Equal(250_000m, g.Valor);
    }

    [Fact]
    public void GastoGeneral_sin_concepto_lanza() =>
        Assert.Throws<DomainException>(() => GastoGeneral.Crear(Hoy, "", 100m));

    [Fact]
    public void GastoGeneral_con_valor_negativo_lanza() =>
        Assert.Throws<DomainException>(() => GastoGeneral.Crear(Hoy, "Combustible", -1m));

    // ── Ingreso ────────────────────────────────────────────────
    [Fact]
    public void Ingreso_crear_valido()
    {
        var i = Ingreso.Crear(Hoy, CentroCostoId.New(), "Venta leche", 1_000_000m);
        Assert.Equal("Venta leche", i.TipoIngreso);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Ingreso_con_valor_no_positivo_lanza(int valor) =>
        Assert.Throws<DomainException>(() => Ingreso.Crear(Hoy, CentroCostoId.New(), "Venta", valor));

    // ── Autoconsumo ────────────────────────────────────────────
    [Fact]
    public void Autoconsumo_con_valor_total_negativo_lanza() =>
        Assert.Throws<DomainException>(() =>
            Autoconsumo.Crear(Hoy, CentroCostoId.New(), "Leche", valorTotal: -1m));

    // ── CostoDiario ────────────────────────────────────────────
    [Fact]
    public void CostoDiario_sin_tipo_lanza() =>
        Assert.Throws<DomainException>(() => CostoDiario.Crear(Hoy, CentroCostoId.New(), "", 100m));

    [Fact]
    public void CostoDiario_con_valor_negativo_lanza() =>
        Assert.Throws<DomainException>(() => CostoDiario.Crear(Hoy, CentroCostoId.New(), "Mano de obra", -1m));

    // ── CostoAnimalDiario ──────────────────────────────────────
    [Fact]
    public void CostoAnimalDiario_crear_valido()
    {
        var c = CostoAnimalDiario.Crear(AnimalId.New(), Hoy, "Alimentación", 12_000m);
        Assert.Equal(12_000m, c.Valor);
    }

    [Fact]
    public void CostoAnimalDiario_con_valor_negativo_lanza() =>
        Assert.Throws<DomainException>(() => CostoAnimalDiario.Crear(AnimalId.New(), Hoy, "Alimentación", -1m));
}
