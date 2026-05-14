using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

public sealed record AbortoRegistradoEvent(GestacionId IdGestacion, AnimalId IdMadre, DateOnly Fecha) : DomainEvent;
