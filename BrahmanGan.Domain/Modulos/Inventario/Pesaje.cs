using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Inventario;

/// <summary>Pesaje individual de un animal en una fecha dada.</summary>
public sealed class Pesaje : Entity<PesajeId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public decimal PesoKg { get; private set; }
    public decimal? CondicionCorporal { get; private set; }
    public string? MetodoPesaje { get; private set; }
    public string? Responsable { get; private set; }

    private Pesaje() { }

    public static Pesaje Registrar(AnimalId idAnimal, DateOnly fecha, decimal pesoKg,
        decimal? condicionCorporal = null, string? metodoPesaje = null, string? responsable = null)
    {
        if (pesoKg <= 0) throw new DomainException("El peso debe ser mayor a cero");
        if (condicionCorporal is < 1 or > 9)
            throw new DomainException("Condición corporal fuera de rango (escala 1-9)");
        return new Pesaje
        {
            Id = PesajeId.New(),
            IdAnimal = idAnimal,
            Fecha = fecha,
            PesoKg = pesoKg,
            CondicionCorporal = condicionCorporal,
            MetodoPesaje = metodoPesaje,
            Responsable = responsable
        };
    }
}
