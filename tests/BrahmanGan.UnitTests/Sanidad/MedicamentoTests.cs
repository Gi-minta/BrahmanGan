using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Sanidad;
using Xunit;

namespace BrahmanGan.UnitTests.Sanidad;

public class MedicamentoTests
{
    private static readonly DateOnly Aplicacion = new(2026, 6, 1);

    [Fact]
    public void Crear_valido_queda_activo()
    {
        var m = Medicamento.Crear("MED-1", "Oxitetraciclina", tiempoCarne: 21, tiempoLeche: 5);

        Assert.Equal("MED-1", m.Codigo);
        Assert.True(m.Activo);
    }

    [Theory]
    [InlineData("", "Nombre")]
    [InlineData("COD", "")]
    public void Crear_con_datos_requeridos_vacios_lanza(string codigo, string nombre)
    {
        Assert.Throws<DomainException>(() => Medicamento.Crear(codigo, nombre));
    }

    [Fact]
    public void Crear_con_precio_negativo_lanza()
    {
        Assert.Throws<DomainException>(() => Medicamento.Crear("MED-1", "X", precioUnitario: -1m));
    }

    [Fact]
    public void Crear_con_tiempos_de_retiro_negativos_lanza()
    {
        Assert.Throws<DomainException>(() => Medicamento.Crear("MED-1", "X", tiempoCarne: -1));
    }

    [Fact]
    public void FechaLiberacion_suma_los_dias_de_retiro()
    {
        var m = Medicamento.Crear("MED-1", "X", tiempoCarne: 21, tiempoLeche: 5);

        Assert.Equal(Aplicacion.AddDays(21), m.FechaLiberacionCarne(Aplicacion));
        Assert.Equal(Aplicacion.AddDays(5), m.FechaLiberacionLeche(Aplicacion));
    }

    [Fact]
    public void FechaLiberacion_es_null_si_no_hay_tiempo_de_retiro()
    {
        var m = Medicamento.Crear("MED-1", "X");

        Assert.Null(m.FechaLiberacionCarne(Aplicacion));
        Assert.Null(m.FechaLiberacionLeche(Aplicacion));
    }
}
