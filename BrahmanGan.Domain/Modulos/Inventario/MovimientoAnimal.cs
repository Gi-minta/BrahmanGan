using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Inventario;

/// <summary>Movimiento del animal entre centros de costo (no implementado en esta fase, modelado como tipo abierto).</summary>
public sealed class MovimientoAnimal : Entity<MovimientoAnimalId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public TipoMovimientoAnimal TipoMovimiento { get; private set; }
    public DateOnly Fecha { get; private set; }
    public decimal? Valor { get; private set; }
    public int? IdCentro { get; private set; }
    public decimal? PesoKg { get; private set; }
    public string? Observaciones { get; private set; }

    private MovimientoAnimal() { }

    public static MovimientoAnimal Crear(AnimalId idAnimal, TipoMovimientoAnimal tipo, DateOnly fecha,
        decimal? valor = null, int? idCentro = null, decimal? pesoKg = null, string? observaciones = null)
    {
        if (pesoKg is < 0) throw new DomainException("Peso no puede ser negativo");
        return new MovimientoAnimal
        {
            Id = MovimientoAnimalId.New(),
            IdAnimal = idAnimal,
            TipoMovimiento = tipo,
            Fecha = fecha,
            Valor = valor,
            IdCentro = idCentro,
            PesoKg = pesoKg,
            Observaciones = observaciones
        };
    }
}
