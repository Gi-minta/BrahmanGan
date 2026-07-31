using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Equipos;
using Xunit;

namespace BrahmanGan.UnitTests.Equipos;

public class MantenimientoEquipoTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    [Fact]
    public void Registrar_valido_calcula_costo_total()
    {
        var m = MantenimientoEquipo.Registrar(MaquinariaId.New(), Hoy, TipoMantenimiento.PREVENTIVO,
            "Cambio de aceite", costoManoObra: 80_000m, costoRepuestos: 120_000m);

        Assert.Equal(200_000m, m.CostoTotal);
    }

    [Fact]
    public void Registrar_sin_descripcion_lanza()
    {
        Assert.Throws<DomainException>(() =>
            MantenimientoEquipo.Registrar(MaquinariaId.New(), Hoy, TipoMantenimiento.CORRECTIVO, ""));
    }

    [Fact]
    public void Registrar_con_costos_negativos_lanza()
    {
        Assert.Throws<DomainException>(() =>
            MantenimientoEquipo.Registrar(MaquinariaId.New(), Hoy, TipoMantenimiento.PREVENTIVO,
                "Servicio", costoManoObra: -1m));
    }

    [Fact]
    public void Registrar_con_proximo_mantenimiento_anterior_lanza()
    {
        Assert.Throws<BusinessRuleException>(() =>
            MantenimientoEquipo.Registrar(MaquinariaId.New(), Hoy, TipoMantenimiento.PREVENTIVO,
                "Servicio", proximoMantenimiento: Hoy.AddDays(-1)));
    }
}
