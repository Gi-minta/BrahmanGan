using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

public sealed record GestacionIniciadaEvent(GestacionId IdGestacion, AnimalId IdAnimal, DateOnly FechaPartoEstimado) : DomainEvent;
