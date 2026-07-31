using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Trazabilidad;
using Xunit;

namespace BrahmanGan.UnitTests.Trazabilidad;

public class RegistroICATests
{
    private static readonly DateOnly Expedicion = new(2026, 1, 1);

    private static RegistroICA Emitir(DateOnly? vencimiento = null) =>
        RegistroICA.Emitir(AnimalId.New(), "Guía de movilización", "GM-001", Expedicion,
            fechaVencimiento: vencimiento);

    [Fact]
    public void Emitir_valido_queda_vigente()
    {
        var r = Emitir();

        Assert.Equal(EstadoRegistroICA.VIGENTE, r.Estado);
        Assert.Equal("GM-001", r.NumeroDocumento);
    }

    [Theory]
    [InlineData("", "GM-001")]
    [InlineData("Guía", "")]
    public void Emitir_con_datos_requeridos_vacios_lanza(string tipo, string numero)
    {
        Assert.Throws<DomainException>(() =>
            RegistroICA.Emitir(AnimalId.New(), tipo, numero, Expedicion));
    }

    [Fact]
    public void Emitir_con_vencimiento_anterior_a_expedicion_lanza()
    {
        Assert.Throws<BusinessRuleException>(() => Emitir(vencimiento: Expedicion.AddDays(-1)));
    }

    [Fact]
    public void Cancelar_cambia_estado()
    {
        var r = Emitir();

        r.Cancelar();

        Assert.Equal(EstadoRegistroICA.CANCELADO, r.Estado);
    }

    [Fact]
    public void Cancelar_dos_veces_lanza()
    {
        var r = Emitir();
        r.Cancelar();

        Assert.Throws<BusinessRuleException>(() => r.Cancelar());
    }

    [Fact]
    public void EvaluarVencimiento_marca_vencido_si_paso_la_fecha()
    {
        var r = Emitir(vencimiento: Expedicion.AddDays(30));

        r.EvaluarVencimiento(Expedicion.AddDays(31));

        Assert.Equal(EstadoRegistroICA.VENCIDO, r.Estado);
    }

    [Fact]
    public void EvaluarVencimiento_no_cambia_si_no_ha_vencido()
    {
        var r = Emitir(vencimiento: Expedicion.AddDays(30));

        r.EvaluarVencimiento(Expedicion.AddDays(10));

        Assert.Equal(EstadoRegistroICA.VIGENTE, r.Estado);
    }

    [Fact]
    public void EvaluarVencimiento_no_afecta_a_un_registro_cancelado()
    {
        var r = Emitir(vencimiento: Expedicion.AddDays(30));
        r.Cancelar();

        r.EvaluarVencimiento(Expedicion.AddDays(100));

        Assert.Equal(EstadoRegistroICA.CANCELADO, r.Estado);
    }

    [Fact]
    public void RequiereAlerta_true_si_el_vencimiento_esta_dentro_del_umbral()
    {
        var r = Emitir(vencimiento: Expedicion.AddDays(40));

        Assert.True(r.RequiereAlerta(Expedicion.AddDays(20), diasUmbral: 30));
    }

    [Fact]
    public void RequiereAlerta_false_si_el_vencimiento_esta_lejos()
    {
        var r = Emitir(vencimiento: Expedicion.AddDays(200));

        Assert.False(r.RequiereAlerta(Expedicion.AddDays(20), diasUmbral: 30));
    }

    [Fact]
    public void RequiereAlerta_false_sin_fecha_de_vencimiento()
    {
        var r = Emitir(vencimiento: null);

        Assert.False(r.RequiereAlerta(Expedicion.AddDays(20)));
    }
}
