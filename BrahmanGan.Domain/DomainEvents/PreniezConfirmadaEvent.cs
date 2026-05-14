using BrahmanGan.Domain.Common;

namespace BrahmanGan.Domain.DomainEvents;

public sealed record PreniezConfirmadaEvent(ServicioId IdServicio, AnimalId IdHembra, bool Preñada) : DomainEvent;
