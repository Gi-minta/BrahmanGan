using BrahmanGan.Domain.Exceptions;
using Xunit;
using FincaEntity = BrahmanGan.Domain.Modulos.Finca.Finca;

namespace BrahmanGan.UnitTests.Finca;

public class FincaTests
{
    [Fact]
    public void Crear_valida_queda_activa()
    {
        var f = FincaEntity.Crear("La Esperanza", areaHectareas: 120m);

        Assert.True(f.Activa);
        Assert.Equal("La Esperanza", f.Nombre);
        Assert.Equal(120m, f.AreaHectareas);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Crear_sin_nombre_lanza(string nombre)
    {
        Assert.Throws<DomainException>(() => FincaEntity.Crear(nombre));
    }

    [Fact]
    public void Crear_con_area_negativa_lanza()
    {
        Assert.Throws<DomainException>(() => FincaEntity.Crear("La Esperanza", areaHectareas: -1m));
    }

    [Fact]
    public void Desactivar_y_activar_cambian_el_estado()
    {
        var f = FincaEntity.Crear("La Esperanza");

        f.Desactivar();
        Assert.False(f.Activa);

        f.Activar();
        Assert.True(f.Activa);
    }
}
