using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.Modulos.Comercial;

/// <summary>Detalle (animal cotizado) de una cotización de venta.</summary>
public sealed class DetalleCotizacion : Entity<DetalleCotizacionId>
{
    public CotizacionVentaId IdCotizacion { get; private set; } = null!;
    public AnimalId IdAnimal { get; private set; } = null!;
    public decimal? PesoEstimadoKg { get; private set; }
    public decimal? PrecioUnitario { get; private set; }

    private DetalleCotizacion() { }

    internal static DetalleCotizacion Crear(CotizacionVentaId idCot, AnimalId idAnimal, decimal? peso, decimal? precio)
    {
        return new DetalleCotizacion
        {
            Id = DetalleCotizacionId.New(),
            IdCotizacion = idCot, IdAnimal = idAnimal, PesoEstimadoKg = peso, PrecioUnitario = precio
        };
    }
}
