using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Domain.DomainEvents;

public sealed record AnimalEstadoCambiadoEvent(AnimalId IdAnimal, EstadoAnimal Anterior, EstadoAnimal Nuevo) : DomainEvent;
