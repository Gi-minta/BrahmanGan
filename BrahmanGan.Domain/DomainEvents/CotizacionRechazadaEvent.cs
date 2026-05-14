using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;
using BrahmanGan.Domain.Modulos.Comercial;

namespace BrahmanGan.Domain.DomainEvents;
public sealed record CotizacionRechazadaEvent(CotizacionVentaId IdCotizacion, ClienteId IdCliente) : DomainEvent;
