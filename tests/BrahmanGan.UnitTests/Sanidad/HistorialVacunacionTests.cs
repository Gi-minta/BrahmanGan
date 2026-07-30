using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Sanidad;
using Xunit;

namespace BrahmanGan.UnitTests.Sanidad;

public class HistorialVacunacionTests
{
    private static readonly DateOnly Hoy = new(2026, 6, 1);

    private static HistorialVacunacion Aplicar(DateOnly? proxima = null) =>
        HistorialVacunacion.Aplicar(AnimalId.New(), MedicamentoId.New(), Hoy, dosis: 2m, proximaFecha: proxima);

    [Fact]
    public void Aplicar_valida_fija_datos()
    {
        var v = Aplicar();

        Assert.Equal(Hoy, v.Fecha);
        Assert.Equal(2m, v.Dosis);
    }

    [Fact]
    public void Aplicar_con_dosis_negativa_lanza()
    {
        Assert.Throws<DomainException>(() =>
            HistorialVacunacion.Aplicar(AnimalId.New(), MedicamentoId.New(), Hoy, dosis: -1m));
    }

    [Fact]
    public void Aplicar_con_proxima_fecha_anterior_lanza()
    {
        Assert.Throws<BusinessRuleException>(() =>
            HistorialVacunacion.Aplicar(AnimalId.New(), MedicamentoId.New(), Hoy, proximaFecha: Hoy.AddDays(-1)));
    }

    [Fact]
    public void RequiereAlerta_true_si_proxima_dentro_del_umbral()
    {
        var v = Aplicar(proxima: Hoy.AddDays(5));

        Assert.True(v.RequiereAlerta(Hoy, diasUmbral: 7));
    }

    [Fact]
    public void RequiereAlerta_false_si_proxima_fuera_del_umbral()
    {
        var v = Aplicar(proxima: Hoy.AddDays(30));

        Assert.False(v.RequiereAlerta(Hoy, diasUmbral: 7));
    }

    [Fact]
    public void RequiereAlerta_false_sin_proxima_fecha()
    {
        var v = Aplicar(proxima: null);

        Assert.False(v.RequiereAlerta(Hoy));
    }
}
