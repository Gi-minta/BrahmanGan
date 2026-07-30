using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Reproduccion;
using Xunit;

namespace BrahmanGan.UnitTests;

public class GestacionTests
{
    private static readonly DateOnly Inicio = new(2026, 1, 1);

    [Fact]
    public void Iniciar_calcula_parto_estimado_a_283_dias_y_emite_evento()
    {
        var g = Gestacion.Iniciar(AnimalId.New(), Inicio);

        Assert.Equal(EstadoGestacion.EN_CURSO, g.Estado);
        Assert.Equal(Inicio.AddDays(Gestacion.DiasGestacionBovino), g.FechaPartoEstimado);
        Assert.Contains(g.DomainEvents, e => e.GetType().Name == "GestacionIniciadaEvent");
    }

    [Fact]
    public void RegistrarParto_valido_cambia_estado_y_fija_fecha()
    {
        var g = Gestacion.Iniciar(AnimalId.New(), Inicio);
        var fechaParto = Inicio.AddDays(280);

        g.RegistrarParto(fechaParto);

        Assert.Equal(EstadoGestacion.PARTO, g.Estado);
        Assert.Equal(fechaParto, g.FechaPartoReal);
        Assert.Contains(g.DomainEvents, e => e.GetType().Name == "PartoRegistradoEvent");
    }

    [Fact]
    public void RegistrarParto_con_fecha_anterior_al_inicio_lanza()
    {
        var g = Gestacion.Iniciar(AnimalId.New(), Inicio);

        Assert.Throws<BusinessRuleException>(() => g.RegistrarParto(Inicio.AddDays(-1)));
    }

    [Fact]
    public void RegistrarParto_sobre_gestacion_no_en_curso_lanza()
    {
        var g = Gestacion.Iniciar(AnimalId.New(), Inicio);
        g.RegistrarParto(Inicio.AddDays(283));

        Assert.Throws<BusinessRuleException>(() => g.RegistrarParto(Inicio.AddDays(284)));
    }

    [Fact]
    public void RegistrarAborto_valido_cambia_estado()
    {
        var g = Gestacion.Iniciar(AnimalId.New(), Inicio);

        g.RegistrarAborto(Inicio.AddDays(100), "causa X");

        Assert.Equal(EstadoGestacion.ABORTO, g.Estado);
        Assert.Contains(g.DomainEvents, e => e.GetType().Name == "AbortoRegistradoEvent");
    }

    [Fact]
    public void Interrumpir_sin_motivo_lanza()
    {
        var g = Gestacion.Iniciar(AnimalId.New(), Inicio);

        Assert.Throws<DomainException>(() => g.Interrumpir(""));
    }

    [Fact]
    public void Interrumpir_valido_cambia_estado()
    {
        var g = Gestacion.Iniciar(AnimalId.New(), Inicio);

        g.Interrumpir("decisión sanitaria");

        Assert.Equal(EstadoGestacion.INTERRUMPIDA, g.Estado);
    }
}
