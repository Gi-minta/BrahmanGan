using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

// ===== Leche =====
public sealed record ProduccionLecheRegistradaEvent(ProduccionLecheId Id, FincaId IdFinca, DateOnly Fecha, decimal Litros) : DomainEvent;
