using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Nomina;
using Xunit;

namespace BrahmanGan.UnitTests.Nomina;

public class PagoJornalTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    [Fact]
    public void Registrar_valido_fija_datos()
    {
        var p = PagoJornal.Registrar(TrabajadorId.New(), Hoy, valorJornal: 60_000m, horasTrabajadas: 8m);

        Assert.Equal(60_000m, p.ValorJornal);
        Assert.Equal(8m, p.HorasTrabajadas);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Registrar_con_valor_jornal_no_positivo_lanza(int valor)
    {
        Assert.Throws<DomainException>(() =>
            PagoJornal.Registrar(TrabajadorId.New(), Hoy, valor));
    }

    [Fact]
    public void Registrar_con_horas_negativas_lanza()
    {
        Assert.Throws<DomainException>(() =>
            PagoJornal.Registrar(TrabajadorId.New(), Hoy, valorJornal: 60_000m, horasTrabajadas: -1m));
    }
}
