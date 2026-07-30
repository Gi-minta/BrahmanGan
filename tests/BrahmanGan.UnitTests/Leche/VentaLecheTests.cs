using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Leche;
using Xunit;

namespace BrahmanGan.UnitTests.Leche;

public class VentaLecheTests
{
    private static readonly DateOnly Hoy = new(2026, 5, 1);

    [Fact]
    public void Registrar_valida_calcula_total_y_emite_evento()
    {
        var v = VentaLeche.Registrar(Hoy, ClienteId.New(), litros: 10m, precioLitro: 2.5m);

        Assert.Equal(10m, v.LitrosVendidos);
        Assert.Equal(2.5m, v.PrecioLitro);
        Assert.Equal(25m, v.TotalVenta);
        Assert.Contains(v.DomainEvents, e => e.GetType().Name == "VentaLecheRegistradaEvent");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Registrar_litros_no_positivos_lanza(int litros)
    {
        Assert.Throws<DomainException>(() =>
            VentaLeche.Registrar(Hoy, ClienteId.New(), litros, precioLitro: 2m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Registrar_precio_no_positivo_lanza(int precio)
    {
        Assert.Throws<DomainException>(() =>
            VentaLeche.Registrar(Hoy, ClienteId.New(), litros: 5m, precioLitro: precio));
    }
}
