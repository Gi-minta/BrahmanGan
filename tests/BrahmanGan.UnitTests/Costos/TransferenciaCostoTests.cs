using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Costos;
using Xunit;

namespace BrahmanGan.UnitTests.Costos;

public class TransferenciaCostoTests
{
    private static readonly DateOnly Hoy = new(2026, 8, 1);

    private static TransferenciaCosto Crear() =>
        TransferenciaCosto.Crear(Hoy, CentroCostoId.From(1), CentroCostoId.From(2), "Reparto", 500_000m);

    [Fact]
    public void Crear_valida_queda_no_aprobada()
    {
        var t = Crear();

        Assert.False(t.Aprobado);
        Assert.Equal(500_000m, t.Valor);
    }

    [Fact]
    public void Crear_con_mismo_centro_origen_y_destino_lanza()
    {
        Assert.Throws<BusinessRuleException>(() =>
            TransferenciaCosto.Crear(Hoy, CentroCostoId.From(1), CentroCostoId.From(1), "X", 100m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Crear_con_valor_no_positivo_lanza(int valor)
    {
        Assert.Throws<DomainException>(() =>
            TransferenciaCosto.Crear(Hoy, CentroCostoId.From(1), CentroCostoId.From(2), "X", valor));
    }

    [Fact]
    public void Aprobar_marca_como_aprobada()
    {
        var t = Crear();

        t.Aprobar();

        Assert.True(t.Aprobado);
    }

    [Fact]
    public void Aprobar_dos_veces_lanza()
    {
        var t = Crear();
        t.Aprobar();

        Assert.Throws<BusinessRuleException>(() => t.Aprobar());
    }
}
