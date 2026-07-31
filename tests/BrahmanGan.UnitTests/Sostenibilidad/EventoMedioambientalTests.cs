using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Sostenibilidad;
using Xunit;

namespace BrahmanGan.UnitTests.Sostenibilidad;

public class EventoMedioambientalTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    [Fact]
    public void Registrar_valido_fija_datos()
    {
        var e = EventoMedioambiental.Registrar(FincaId.New(), Hoy, "Sequía",
            tempMaxC: 38m, tempMinC: 22m);

        Assert.Equal("Sequía", e.TipoEvento);
        Assert.Equal(38m, e.TempMaxC);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Registrar_sin_tipo_de_evento_lanza(string tipo)
    {
        Assert.Throws<DomainException>(() => EventoMedioambiental.Registrar(FincaId.New(), Hoy, tipo));
    }

    [Fact]
    public void Registrar_con_precipitacion_negativa_lanza()
    {
        Assert.Throws<DomainException>(() =>
            EventoMedioambiental.Registrar(FincaId.New(), Hoy, "Lluvia", precipitacionMM: -1m));
    }

    [Fact]
    public void Registrar_con_temp_max_menor_que_min_lanza()
    {
        Assert.Throws<BusinessRuleException>(() =>
            EventoMedioambiental.Registrar(FincaId.New(), Hoy, "Ola de calor", tempMaxC: 20m, tempMinC: 30m));
    }
}
