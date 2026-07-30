using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Comercial;
using Xunit;

namespace BrahmanGan.UnitTests.Comercial;

public class ContratoTests
{
    private static readonly DateOnly Inicio = new(2026, 1, 1);

    private static Contrato Crear() =>
        Contrato.Crear(ClienteId.New(), "Suministro leche", Inicio, precioAcordado: 2000m);

    [Fact]
    public void Crear_valido_queda_vigente()
    {
        var c = Crear();

        Assert.Equal("VIGENTE", c.Estado);
        Assert.Equal("Suministro leche", c.Tipo);
    }

    [Fact]
    public void Crear_sin_tipo_lanza()
    {
        Assert.Throws<DomainException>(() => Contrato.Crear(ClienteId.New(), "", Inicio));
    }

    [Fact]
    public void Crear_con_fecha_fin_anterior_al_inicio_lanza()
    {
        Assert.Throws<BusinessRuleException>(() =>
            Contrato.Crear(ClienteId.New(), "X", Inicio, fechaFin: Inicio.AddDays(-1)));
    }

    [Fact]
    public void Crear_con_precio_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Contrato.Crear(ClienteId.New(), "X", Inicio, precioAcordado: -1m));
    }

    [Fact]
    public void Cerrar_fija_fecha_fin_y_estado()
    {
        var c = Crear();

        c.Cerrar(Inicio.AddDays(180));

        Assert.Equal("CERRADO", c.Estado);
        Assert.Equal(Inicio.AddDays(180), c.FechaFin);
    }

    [Fact]
    public void Cerrar_con_fecha_anterior_al_inicio_lanza()
    {
        var c = Crear();

        Assert.Throws<BusinessRuleException>(() => c.Cerrar(Inicio.AddDays(-1)));
    }

    [Fact]
    public void Cancelar_un_contrato_vigente_lo_cancela()
    {
        var c = Crear();

        c.Cancelar();

        Assert.Equal("CANCELADO", c.Estado);
    }

    [Fact]
    public void Cancelar_un_contrato_no_vigente_lanza()
    {
        var c = Crear();
        c.Cerrar(Inicio.AddDays(30));

        Assert.Throws<BusinessRuleException>(() => c.Cancelar());
    }
}
