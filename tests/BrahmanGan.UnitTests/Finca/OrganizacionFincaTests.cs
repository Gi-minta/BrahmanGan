using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Finca;
using Xunit;

namespace BrahmanGan.UnitTests.Finca;

/// <summary>Tests de GrupoManejo, Zona y ZonaFinca.</summary>
public class OrganizacionFincaTests
{
    private static readonly DateOnly Ingreso = new(2026, 4, 1);

    // ── GrupoManejo ────────────────────────────────────────────
    [Fact]
    public void GrupoManejo_crear_normaliza_codigo()
    {
        var g = GrupoManejo.Crear("vl", "Vacas lactancia");
        Assert.Equal("VL", g.Codigo);
    }

    [Theory]
    [InlineData("", "Nombre")]
    [InlineData("G1", "")]
    public void GrupoManejo_datos_vacios_lanza(string codigo, string nombre)
    {
        Assert.Throws<DomainException>(() => GrupoManejo.Crear(codigo, nombre));
    }

    // ── Zona ───────────────────────────────────────────────────
    [Fact]
    public void Zona_crear_normaliza_codigo_y_queda_activa()
    {
        var z = Zona.Crear("rt-1", "Ruta lechera 1", tipo: "Recolección");
        Assert.Equal("RT-1", z.Codigo);
        Assert.True(z.Activa);
    }

    [Theory]
    [InlineData("", "Nombre")]
    [InlineData("Z1", "")]
    public void Zona_datos_vacios_lanza(string codigo, string nombre)
    {
        Assert.Throws<DomainException>(() => Zona.Crear(codigo, nombre));
    }

    // ── ZonaFinca ──────────────────────────────────────────────
    [Fact]
    public void ZonaFinca_crear_usa_fecha_de_ingreso_indicada()
    {
        var zf = ZonaFinca.Crear(ZonaId.New(), FincaId.New(), fechaIngreso: Ingreso);
        Assert.Equal(Ingreso, zf.FechaIngreso);
        Assert.Null(zf.FechaSalida);
    }

    [Fact]
    public void ZonaFinca_cerrar_con_fecha_anterior_al_ingreso_lanza()
    {
        var zf = ZonaFinca.Crear(ZonaId.New(), FincaId.New(), fechaIngreso: Ingreso);

        Assert.Throws<BusinessRuleException>(() => zf.Cerrar(Ingreso.AddDays(-1)));
    }

    [Fact]
    public void ZonaFinca_cerrar_valido_fija_salida()
    {
        var zf = ZonaFinca.Crear(ZonaId.New(), FincaId.New(), fechaIngreso: Ingreso);

        zf.Cerrar(Ingreso.AddDays(60));

        Assert.Equal(Ingreso.AddDays(60), zf.FechaSalida);
    }
}
