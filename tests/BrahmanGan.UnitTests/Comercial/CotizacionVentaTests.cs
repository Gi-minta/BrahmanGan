using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Comercial;
using Xunit;

namespace BrahmanGan.UnitTests.Comercial;

public class CotizacionVentaTests
{
    private static readonly DateOnly Fecha = new(2026, 2, 1);

    private static CotizacionVenta Crear() =>
        CotizacionVenta.Crear(ClienteId.New(), Fecha, precioOfertado: 1500m);

    [Fact]
    public void Crear_valida_queda_pendiente()
    {
        var c = Crear();

        Assert.Equal("PENDIENTE", c.Estado);
        Assert.Empty(c.Detalles);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Crear_con_precio_no_positivo_lanza(int precio)
    {
        Assert.Throws<DomainException>(() =>
            CotizacionVenta.Crear(ClienteId.New(), Fecha, precio));
    }

    [Fact]
    public void Crear_con_vigencia_anterior_a_la_fecha_lanza()
    {
        Assert.Throws<BusinessRuleException>(() =>
            CotizacionVenta.Crear(ClienteId.New(), Fecha, 1500m, fechaVigencia: Fecha.AddDays(-1)));
    }

    [Fact]
    public void AgregarDetalle_agrega_a_la_coleccion()
    {
        var c = Crear();

        c.AgregarDetalle(AnimalId.New(), pesoEstimado: 420m, precioUnitario: 3m);

        Assert.Single(c.Detalles);
    }

    [Fact]
    public void AgregarDetalle_con_valores_negativos_lanza()
    {
        var c = Crear();

        Assert.Throws<DomainException>(() =>
            c.AgregarDetalle(AnimalId.New(), pesoEstimado: -1m));
    }

    [Fact]
    public void Aprobar_cambia_estado_y_emite_evento()
    {
        var c = Crear();

        c.Aprobar();

        Assert.Equal("APROBADA", c.Estado);
        Assert.Contains(c.DomainEvents, e => e.GetType().Name == "CotizacionAprobadaEvent");
    }

    [Fact]
    public void Rechazar_cambia_estado_y_emite_evento()
    {
        var c = Crear();

        c.Rechazar();

        Assert.Equal("RECHAZADA", c.Estado);
        Assert.Contains(c.DomainEvents, e => e.GetType().Name == "CotizacionRechazadaEvent");
    }

    [Fact]
    public void AgregarDetalle_sobre_cotizacion_no_pendiente_lanza()
    {
        var c = Crear();
        c.Aprobar();

        Assert.Throws<BusinessRuleException>(() => c.AgregarDetalle(AnimalId.New()));
    }

    [Fact]
    public void Aprobar_una_cotizacion_no_pendiente_lanza()
    {
        var c = Crear();
        c.Rechazar();

        Assert.Throws<BusinessRuleException>(() => c.Aprobar());
    }
}
