using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Reproduccion;

/// <summary>Nacimiento (cría) ligado a una gestación. Puede o no asociarse a un Animal recién registrado.</summary>
public sealed class Nacimiento : Entity<NacimientoId>
{
    public GestacionId IdGestacion { get; private set; } = null!;
    public AnimalId? IdAnimalCria { get; private set; }
    public DateOnly Fecha { get; private set; }
    public char? Sexo { get; private set; }
    public decimal? PesoNacimiento { get; private set; }
    public string? Condicion { get; private set; }
    public string? Observaciones { get; private set; }

    private Nacimiento() { }

    public static Nacimiento Registrar(GestacionId idGestacion, DateOnly fecha,
        char? sexo = null, decimal? pesoNacimiento = null, string? condicion = null,
        AnimalId? idAnimalCria = null, string? observaciones = null)
    {
        if (idGestacion is null) throw new DomainException("Gestación requerida");
        if (sexo.HasValue && sexo != 'M' && sexo != 'H')
            throw new DomainException("Sexo debe ser 'M' o 'H'");
        if (pesoNacimiento is < 0) throw new DomainException("Peso no puede ser negativo");
        return new Nacimiento
        {
            Id = NacimientoId.New(),
            IdGestacion = idGestacion,
            Fecha = fecha,
            Sexo = sexo,
            PesoNacimiento = pesoNacimiento,
            Condicion = condicion,
            IdAnimalCria = idAnimalCria,
            Observaciones = observaciones
        };
    }

    public void VincularCria(AnimalId idCria)
    {
        if (idCria is null) throw new DomainException("Cría requerida");
        IdAnimalCria = idCria;
        MarkAsModified();
    }
}
