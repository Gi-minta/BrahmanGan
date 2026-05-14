using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Leche;

/// <summary>
/// Control individual de leche de un animal en una fecha específica.
/// TotalLitros = mañana + tarde + noche (campo computado en BD).
/// </summary>
public sealed class ControlLecheAnimal : Entity<ControlLecheAnimalId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public string? Ordeno { get; private set; }
    public decimal? LitrosManiana { get; private set; }
    public decimal? LitrosTarde { get; private set; }
    public decimal? LitrosNoche { get; private set; }

    /// <summary>Computado en BD; en memoria también lo exponemos.</summary>
    public decimal TotalLitros =>
        (LitrosManiana ?? 0m) + (LitrosTarde ?? 0m) + (LitrosNoche ?? 0m);

    private ControlLecheAnimal() { }

    public static ControlLecheAnimal Registrar(AnimalId idAnimal, DateOnly fecha,
        decimal? maniana = null, decimal? tarde = null, decimal? noche = null, string? ordeno = null)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        if (maniana is < 0 || tarde is < 0 || noche is < 0)
            throw new DomainException("Litros no pueden ser negativos");
        if ((maniana ?? 0) + (tarde ?? 0) + (noche ?? 0) <= 0)
            throw new DomainException("Debe registrarse al menos un ordeño con litros > 0");
        return new ControlLecheAnimal
        {
            Id = ControlLecheAnimalId.New(),
            IdAnimal = idAnimal,
            Fecha = fecha,
            Ordeno = ordeno,
            LitrosManiana = maniana,
            LitrosTarde = tarde,
            LitrosNoche = noche
        };
    }
}
