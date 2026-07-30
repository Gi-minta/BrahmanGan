using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Sanidad;
using Xunit;

namespace BrahmanGan.UnitTests.Sanidad;

public class HistorialCurativoTests
{
    private static readonly DateOnly Inicio = new(2026, 6, 1);

    private static HistorialCurativo Iniciar() =>
        HistorialCurativo.Iniciar(AnimalId.New(), "Fiebre", Inicio, veterinario: "Dr. Vet");

    [Fact]
    public void Iniciar_valida_fija_datos()
    {
        var h = Iniciar();

        Assert.Equal("Fiebre", h.Diagnostico);
        Assert.Null(h.FechaFin);
        Assert.Empty(h.Detalles);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Iniciar_sin_diagnostico_lanza(string diagnostico)
    {
        Assert.Throws<DomainException>(() =>
            HistorialCurativo.Iniciar(AnimalId.New(), diagnostico, Inicio));
    }

    [Fact]
    public void AgregarDetalle_recalcula_el_costo_total()
    {
        var h = Iniciar();

        h.AgregarDetalle(MedicamentoId.New(), Inicio, dosis: 2m, costoUnitario: 10m);
        h.AgregarDetalle(MedicamentoId.New(), Inicio.AddDays(1), dosis: 1m, costoUnitario: 5m);

        Assert.Equal(2, h.Detalles.Count);
        Assert.Equal(25m, h.CostoTotal); // 2*10 + 1*5
    }

    [Fact]
    public void AgregarDetalle_con_fecha_anterior_al_inicio_lanza()
    {
        var h = Iniciar();

        Assert.Throws<BusinessRuleException>(() =>
            h.AgregarDetalle(MedicamentoId.New(), Inicio.AddDays(-1)));
    }

    [Fact]
    public void AgregarDetalle_sobre_tratamiento_cerrado_lanza()
    {
        var h = Iniciar();
        h.Cerrar(Inicio.AddDays(3), "Recuperado");

        Assert.Throws<BusinessRuleException>(() =>
            h.AgregarDetalle(MedicamentoId.New(), Inicio.AddDays(4)));
    }

    [Fact]
    public void Cerrar_valida_fija_fin_y_resultado()
    {
        var h = Iniciar();

        h.Cerrar(Inicio.AddDays(3), "Recuperado");

        Assert.Equal(Inicio.AddDays(3), h.FechaFin);
        Assert.Equal("Recuperado", h.Resultado);
    }

    [Fact]
    public void Cerrar_dos_veces_lanza()
    {
        var h = Iniciar();
        h.Cerrar(Inicio.AddDays(3), "Recuperado");

        Assert.Throws<BusinessRuleException>(() => h.Cerrar(Inicio.AddDays(4), "Otro"));
    }

    [Fact]
    public void Cerrar_con_fecha_fin_anterior_al_inicio_lanza()
    {
        var h = Iniciar();

        Assert.Throws<BusinessRuleException>(() => h.Cerrar(Inicio.AddDays(-1), "x"));
    }
}
