using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Sanidad;
using Xunit;

namespace BrahmanGan.UnitTests.Sanidad;

public class HistorialPreventivoTests
{
    private static readonly DateOnly Hoy = new(2026, 6, 1);

    [Fact]
    public void Aplicar_valido_fija_datos()
    {
        var p = HistorialPreventivo.Aplicar(AnimalId.New(), ControlPreventivoId.New(), Hoy, dosis: 3m);

        Assert.Equal(Hoy, p.Fecha);
        Assert.Equal(3m, p.Dosis);
    }

    [Fact]
    public void Aplicar_con_dosis_negativa_lanza()
    {
        Assert.Throws<DomainException>(() =>
            HistorialPreventivo.Aplicar(AnimalId.New(), ControlPreventivoId.New(), Hoy, dosis: -1m));
    }

    [Fact]
    public void Aplicar_con_proxima_fecha_anterior_lanza()
    {
        Assert.Throws<BusinessRuleException>(() =>
            HistorialPreventivo.Aplicar(AnimalId.New(), ControlPreventivoId.New(), Hoy, proximaFecha: Hoy.AddDays(-1)));
    }
}
