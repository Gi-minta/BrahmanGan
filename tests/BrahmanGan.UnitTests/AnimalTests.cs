using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Inventario;
using Xunit;

namespace BrahmanGan.UnitTests;

public class AnimalTests
{
    private static Animal NuevoActivo(Sexo sexo = Sexo.H, int finca = 1) =>
        Animal.Registrar("A001", sexo, RazaId.New(), FincaId.From(finca));

    [Fact]
    public void Registrar_valido_queda_activo_con_Id_y_evento()
    {
        var a = NuevoActivo();

        Assert.Equal(EstadoAnimal.ACTIVO, a.Estado);
        Assert.NotNull(a.Id);
        Assert.Contains(a.DomainEvents, e => e.GetType().Name == "AnimalRegistradoEvent");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Registrar_codigo_vacio_lanza(string codigo)
    {
        Assert.Throws<DomainException>(() =>
            Animal.Registrar(codigo, Sexo.H, RazaId.New(), FincaId.New()));
    }

    [Fact]
    public void Registrar_codigo_muy_largo_lanza()
    {
        var codigo = new string('X', 21);
        Assert.Throws<DomainException>(() =>
            Animal.Registrar(codigo, Sexo.H, RazaId.New(), FincaId.New()));
    }

    [Fact]
    public void Registrar_fecha_nacimiento_futura_lanza()
    {
        var futura = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);
        Assert.Throws<DomainException>(() =>
            Animal.Registrar("A001", Sexo.H, RazaId.New(), FincaId.New(), fechaNacimiento: futura));
    }

    [Fact]
    public void Registrar_peso_negativo_lanza()
    {
        Assert.Throws<DomainException>(() =>
            Animal.Registrar("A001", Sexo.H, RazaId.New(), FincaId.New(), pesoNacimiento: -1m));
    }

    [Fact]
    public void CambiarEstado_a_muerto_emite_evento_y_bloquea_reactivacion()
    {
        var a = NuevoActivo();

        a.CambiarEstado(EstadoAnimal.MUERTO);

        Assert.Equal(EstadoAnimal.MUERTO, a.Estado);
        Assert.Contains(a.DomainEvents, e => e.GetType().Name == "AnimalEstadoCambiadoEvent");
        Assert.Throws<BusinessRuleException>(() => a.CambiarEstado(EstadoAnimal.ACTIVO));
    }

    [Fact]
    public void CambiarEstado_al_mismo_estado_no_emite_evento()
    {
        var a = NuevoActivo();
        a.ClearDomainEvents();

        a.CambiarEstado(EstadoAnimal.ACTIVO);

        Assert.Empty(a.DomainEvents);
    }

    [Fact]
    public void Trasladar_cambia_finca_y_marca_transferido()
    {
        var a = Animal.Registrar("A001", Sexo.H, RazaId.New(), FincaId.From(1));

        a.Trasladar(FincaId.From(2));

        Assert.Equal(2, a.IdFinca.Value);
        Assert.Equal(EstadoAnimal.TRANSFERIDO, a.Estado);
        Assert.Contains(a.DomainEvents, e => e.GetType().Name == "AnimalTrasladadoEvent");
    }

    [Fact]
    public void Trasladar_animal_no_activo_lanza()
    {
        var a = Animal.Registrar("A001", Sexo.H, RazaId.New(), FincaId.From(1));
        a.Trasladar(FincaId.From(2)); // queda TRANSFERIDO

        Assert.Throws<BusinessRuleException>(() => a.Trasladar(FincaId.From(3)));
    }

    [Fact]
    public void AsignarMadre_con_macho_lanza()
    {
        var cria = NuevoActivo(Sexo.H);
        var macho = NuevoActivo(Sexo.M);

        Assert.Throws<BusinessRuleException>(() => cria.AsignarMadre(macho));
    }

    [Fact]
    public void AsignarPadre_con_hembra_lanza()
    {
        var cria = NuevoActivo(Sexo.H);
        var hembra = NuevoActivo(Sexo.H);

        Assert.Throws<BusinessRuleException>(() => cria.AsignarPadre(hembra));
    }
}
