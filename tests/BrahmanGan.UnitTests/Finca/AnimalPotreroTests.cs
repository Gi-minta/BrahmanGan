using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Finca;
using Xunit;

namespace BrahmanGan.UnitTests.Finca;

public class AnimalPotreroTests
{
    private static readonly DateOnly Ingreso = new(2026, 3, 1);

    private static AnimalPotrero Asignar() =>
        AnimalPotrero.Asignar(AnimalId.New(), PotreroId.New(), Ingreso);

    [Fact]
    public void Asignar_valida_queda_vigente()
    {
        var a = Asignar();

        Assert.True(a.EstaVigente());
        Assert.Null(a.FechaSalida);
    }

    [Fact]
    public void Cerrar_fija_salida_y_deja_de_estar_vigente()
    {
        var a = Asignar();

        a.Cerrar(Ingreso.AddDays(20));

        Assert.Equal(Ingreso.AddDays(20), a.FechaSalida);
        Assert.False(a.EstaVigente());
    }

    [Fact]
    public void Cerrar_con_fecha_salida_anterior_al_ingreso_lanza()
    {
        var a = Asignar();

        Assert.Throws<BusinessRuleException>(() => a.Cerrar(Ingreso.AddDays(-1)));
    }
}
