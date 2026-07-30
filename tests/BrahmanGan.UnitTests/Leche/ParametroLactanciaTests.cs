using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Leche;
using Xunit;

namespace BrahmanGan.UnitTests.Leche;

public class ParametroLactanciaTests
{
    private static readonly DateOnly Inicio = new(2026, 1, 10);

    [Fact]
    public void Iniciar_valida_fija_datos()
    {
        var p = ParametroLactancia.Iniciar(AnimalId.New(), numeroParto: 2, Inicio);

        Assert.Equal(2, p.NumeroParto);
        Assert.Equal(Inicio, p.FechaInicio);
        Assert.Null(p.FechaFin);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Iniciar_numero_de_parto_no_positivo_lanza(int numeroParto)
    {
        Assert.Throws<DomainException>(() =>
            ParametroLactancia.Iniciar(AnimalId.New(), numeroParto, Inicio));
    }

    [Fact]
    public void Cerrar_valida_fija_fin_y_litros()
    {
        var p = ParametroLactancia.Iniciar(AnimalId.New(), 1, Inicio);

        p.Cerrar(Inicio.AddDays(300), litrosTotales: 4200m);

        Assert.Equal(Inicio.AddDays(300), p.FechaFin);
        Assert.Equal(4200m, p.LitrosTotales);
    }

    [Fact]
    public void Cerrar_dos_veces_lanza()
    {
        var p = ParametroLactancia.Iniciar(AnimalId.New(), 1, Inicio);
        p.Cerrar(Inicio.AddDays(300));

        Assert.Throws<BusinessRuleException>(() => p.Cerrar(Inicio.AddDays(310)));
    }

    [Fact]
    public void Cerrar_con_fecha_fin_anterior_al_inicio_lanza()
    {
        var p = ParametroLactancia.Iniciar(AnimalId.New(), 1, Inicio);

        Assert.Throws<BusinessRuleException>(() => p.Cerrar(Inicio.AddDays(-1)));
    }

    [Fact]
    public void Cerrar_con_litros_negativos_lanza()
    {
        var p = ParametroLactancia.Iniciar(AnimalId.New(), 1, Inicio);

        Assert.Throws<DomainException>(() => p.Cerrar(Inicio.AddDays(300), litrosTotales: -1m));
    }
}
