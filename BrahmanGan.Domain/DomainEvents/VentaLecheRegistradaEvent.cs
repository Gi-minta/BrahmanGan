using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

public sealed record VentaLecheRegistradaEvent(VentaLecheId Id, ClienteId IdCliente, decimal Total) : DomainEvent;
