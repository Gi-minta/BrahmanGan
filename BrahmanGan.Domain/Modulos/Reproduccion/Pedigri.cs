using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Reproduccion;

/// <summary>Pedigrí del animal: abuelos y puntaje morfológico.</summary>
public sealed class Pedigri : Entity<PedigriId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public AnimalId? IdAbuelo1 { get; private set; }
    public AnimalId? IdAbuela1 { get; private set; }
    public AnimalId? IdAbuelo2 { get; private set; }
    public AnimalId? IdAbuela2 { get; private set; }
    public decimal? PuntajeMorfologia { get; private set; }
    public string? Observaciones { get; private set; }

    private Pedigri() { }

    public static Pedigri Crear(AnimalId idAnimal,
        AnimalId? abuelo1 = null, AnimalId? abuela1 = null,
        AnimalId? abuelo2 = null, AnimalId? abuela2 = null,
        decimal? puntajeMorfologia = null, string? observaciones = null)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        if (puntajeMorfologia is < 0 or > 100)
            throw new DomainException("Puntaje morfología fuera de rango (0-100)");
        return new Pedigri
        {
            Id = PedigriId.New(),
            IdAnimal = idAnimal,
            IdAbuelo1 = abuelo1, IdAbuela1 = abuela1,
            IdAbuelo2 = abuelo2, IdAbuela2 = abuela2,
            PuntajeMorfologia = puntajeMorfologia,
            Observaciones = observaciones
        };
    }
}
