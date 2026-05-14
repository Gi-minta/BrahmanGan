using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

public sealed record PartoRegistradoEvent(GestacionId IdGestacion, AnimalId IdMadre, DateOnly FechaParto) : DomainEvent;
