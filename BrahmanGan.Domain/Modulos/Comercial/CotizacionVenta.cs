using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.DomainEvents;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Comercial;

/// <summary>Cotización de venta a un cliente, con detalle de animales o productos ofertados.</summary>
public sealed class CotizacionVenta : AggregateRoot<CotizacionVentaId>
{
    private readonly List<DetalleCotizacion> _detalles = new();

    public ClienteId IdCliente { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public DateOnly? FechaVigencia { get; private set; }
    public decimal PrecioOfertado { get; private set; }
    public string? UnidadPrecio { get; private set; }
    public string Estado { get; private set; } = "PENDIENTE";
    public string? Observaciones { get; private set; }
    public IReadOnlyCollection<DetalleCotizacion> Detalles => _detalles.AsReadOnly();

    private CotizacionVenta() { }

    public static CotizacionVenta Crear(ClienteId idCliente, DateOnly fecha, decimal precioOfertado,
        DateOnly? fechaVigencia = null, string? unidadPrecio = null, string? observaciones = null)
    {
        if (idCliente is null) throw new DomainException("Cliente requerido");
        if (precioOfertado <= 0) throw new DomainException("Precio ofertado debe ser > 0");
        if (fechaVigencia.HasValue && fechaVigencia.Value < fecha)
            throw new BusinessRuleException("Vigencia anterior a la fecha de cotización");
        return new CotizacionVenta
        {
            Id = CotizacionVentaId.New(),
            IdCliente = idCliente, Fecha = fecha, FechaVigencia = fechaVigencia,
            PrecioOfertado = precioOfertado, UnidadPrecio = unidadPrecio, Estado = "PENDIENTE",
            Observaciones = observaciones
        };
    }

    public void AgregarDetalle(AnimalId idAnimal, decimal? pesoEstimado = null, decimal? precioUnitario = null)
    {
        if (Estado != "PENDIENTE") throw new BusinessRuleException("Solo se editan cotizaciones PENDIENTES");
        if (idAnimal is null) throw new DomainException("Animal requerido");
        if (pesoEstimado is < 0 || precioUnitario is < 0)
            throw new DomainException("Valores no negativos");
        _detalles.Add(DetalleCotizacion.Crear(Id, idAnimal, pesoEstimado, precioUnitario));
        IncrementVersion();
    }

    public void Aprobar()
    {
        if (Estado != "PENDIENTE") throw new BusinessRuleException("Estado inválido para aprobar");
        Estado = "APROBADA";
        AddDomainEvent(new CotizacionAprobadaEvent(Id, IdCliente));
        IncrementVersion();
    }

    public void Rechazar()
    {
        if (Estado != "PENDIENTE") throw new BusinessRuleException("Estado inválido para rechazar");
        Estado = "RECHAZADA";
        AddDomainEvent(new CotizacionRechazadaEvent(Id, IdCliente));
        IncrementVersion();
    }
}
