using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Almacen;

/// <summary>Acumulación de un insumo aplicado en un potrero (ej: fertilizante).</summary>
public sealed class AcumulacionInsumoPotrero : Entity<AcumulacionInsumoPotreroId>
{
    public PotreroId IdPotrero { get; private set; } = null!;
    public InsumoId IdInsumo { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public decimal Cantidad { get; private set; }
    public decimal? CostoUnitario { get; private set; }

    private AcumulacionInsumoPotrero() { }
    public static AcumulacionInsumoPotrero Registrar(PotreroId idPotrero, InsumoId idInsumo, DateOnly fecha,
        decimal cantidad, decimal? costoUnitario = null)
    {
        if (cantidad <= 0) throw new DomainException("Cantidad debe ser > 0");
        if (costoUnitario is < 0) throw new DomainException("Costo no negativo");
        return new AcumulacionInsumoPotrero { Id = AcumulacionInsumoPotreroId.New(),
            IdPotrero = idPotrero, IdInsumo = idInsumo, Fecha = fecha,
            Cantidad = cantidad, CostoUnitario = costoUnitario };
    }
}
