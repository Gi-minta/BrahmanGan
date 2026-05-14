using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

// ===== Inventario / Animal =====
public sealed record AnimalRegistradoEvent(AnimalId IdAnimal, string Codigo, FincaId IdFinca) : DomainEvent;
