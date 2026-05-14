using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Domain.DomainEvents;

// ===== Reproducción =====
public sealed record ServicioRegistradoEvent(ServicioId IdServicio, AnimalId IdHembra, TipoServicio Tipo, DateOnly Fecha) : DomainEvent;
