using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Leche;
using Xunit;

namespace BrahmanGan.UnitTests.Leche;

public class ProduccionLecheTests
{
    private static readonly DateOnly Hoy = new(2026, 5, 1);

    [Fact]
    public void Registrar_valida_fija_total_y_emite_evento()
    {
        var p = ProduccionLeche.Registrar(FincaId.New(), Hoy, 100m, vendidos: 60m, autoconsumo: 10m, merma: 5m);

        Assert.Equal(100m, p.TotalLitros);
        Assert.Equal(60m, p.LitrosVendidos);
        Assert.Contains(p.DomainEvents, e => e.GetType().Name == "ProduccionLecheRegistradaEvent");
    }

    [Fact]
    public void Registrar_total_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            ProduccionLeche.Registrar(FincaId.New(), Hoy, -1m));
    }

    [Fact]
    public void Registrar_desglose_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            ProduccionLeche.Registrar(FincaId.New(), Hoy, 100m, vendidos: -5m));
    }

    [Fact]
    public void Registrar_suma_de_desgloses_mayor_al_total_lanza()
    {
        Assert.Throws<BusinessRuleException>(() =>
            ProduccionLeche.Registrar(FincaId.New(), Hoy, 100m, vendidos: 80m, autoconsumo: 30m));
    }

    [Fact]
    public void Registrar_suma_de_desgloses_igual_al_total_es_valida()
    {
        var p = ProduccionLeche.Registrar(FincaId.New(), Hoy, 100m, vendidos: 70m, autoconsumo: 20m, merma: 10m);

        Assert.Equal(100m, p.TotalLitros);
    }
}
