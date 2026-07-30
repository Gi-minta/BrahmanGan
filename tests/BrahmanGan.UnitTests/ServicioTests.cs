using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Reproduccion;
using Xunit;

namespace BrahmanGan.UnitTests;

public class ServicioTests
{
    private static readonly DateOnly Fecha = new(2026, 3, 1);

    [Fact]
    public void RegistrarMonta_valida_fija_tipo_y_toro()
    {
        var s = Servicio.RegistrarMonta(AnimalId.From(1), AnimalId.From(2), Fecha);

        Assert.Equal(TipoServicio.MONTA, s.TipoServicio);
        Assert.Equal(2, s.IdToro!.Value);
        Assert.Null(s.IdSemen);
        Assert.Contains(s.DomainEvents, e => e.GetType().Name == "ServicioRegistradoEvent");
    }

    [Fact]
    public void RegistrarMonta_con_hembra_igual_a_toro_lanza()
    {
        Assert.Throws<BusinessRuleException>(() =>
            Servicio.RegistrarMonta(AnimalId.From(1), AnimalId.From(1), Fecha));
    }

    [Fact]
    public void RegistrarIA_valida_fija_tipo_y_semen()
    {
        var s = Servicio.RegistrarIA(AnimalId.From(1), SemenId.From(9), Fecha);

        Assert.Equal(TipoServicio.IA, s.TipoServicio);
        Assert.Equal(9, s.IdSemen!.Value);
        Assert.Null(s.IdToro);
    }

    [Fact]
    public void ConfirmarResultado_con_fecha_anterior_al_servicio_lanza()
    {
        var s = Servicio.RegistrarIA(AnimalId.From(1), SemenId.From(9), Fecha);

        Assert.Throws<BusinessRuleException>(() => s.ConfirmarResultado(true, Fecha.AddDays(-1)));
    }

    [Fact]
    public void ConfirmarResultado_valido_registra_prenez_y_evento()
    {
        var s = Servicio.RegistrarIA(AnimalId.From(1), SemenId.From(9), Fecha);

        s.ConfirmarResultado(true, Fecha.AddDays(45));

        Assert.True(s.ResultadoPreniez);
        Assert.Equal(Fecha.AddDays(45), s.FechaConfirmacion);
        Assert.Contains(s.DomainEvents, e => e.GetType().Name == "PreniezConfirmadaEvent");
    }
}
