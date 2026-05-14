using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

public sealed record AnimalTrasladadoEvent(AnimalId IdAnimal, FincaId FincaOrigen, FincaId FincaDestino) : DomainEvent;
