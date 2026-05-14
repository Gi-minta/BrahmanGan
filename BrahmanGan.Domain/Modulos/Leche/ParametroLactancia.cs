using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Leche;

/// <summary>
/// Parámetros de la lactancia n-ésima de una vaca.
/// Único por (IdAnimal, NumeroParto).
/// </summary>
public sealed class ParametroLactancia : Entity<ParametroLactanciaId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public int NumeroParto { get; private set; }
    public DateOnly FechaInicio { get; private set; }
    public DateOnly? FechaFin { get; private set; }
    public decimal? LitrosTotales { get; private set; }

    private ParametroLactancia() { }

    public static ParametroLactancia Iniciar(AnimalId idAnimal, int numeroParto, DateOnly fechaInicio)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        if (numeroParto <= 0) throw new DomainException("Número de parto debe ser > 0");
        return new ParametroLactancia
        {
            Id = ParametroLactanciaId.New(),
            IdAnimal = idAnimal,
            NumeroParto = numeroParto,
            FechaInicio = fechaInicio
        };
    }

    public void Cerrar(DateOnly fechaFin, decimal? litrosTotales = null)
    {
        if (FechaFin.HasValue) throw new BusinessRuleException("Lactancia ya cerrada");
        if (fechaFin < FechaInicio) throw new BusinessRuleException("FechaFin < FechaInicio");
        if (litrosTotales is < 0) throw new DomainException("Litros no negativos");
        FechaFin = fechaFin;
        LitrosTotales = litrosTotales;
        MarkAsModified();
    }
}
