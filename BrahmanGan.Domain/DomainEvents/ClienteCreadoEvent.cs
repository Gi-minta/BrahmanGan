using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

// ===== Comercial =====
public sealed record ClienteCreadoEvent(ClienteId IdCliente, string Documento, string RazonSocial) : DomainEvent;
