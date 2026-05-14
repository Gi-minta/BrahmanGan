using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

public sealed record CotizacionAprobadaEvent(CotizacionVentaId IdCotizacion, ClienteId IdCliente) : DomainEvent;
