using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Application.Ports.Output;

// Fase 5
public interface IControlLecheAnimalRepository : IRepository<ControlLecheAnimal, ControlLecheAnimalId>
{
    Task<IReadOnlyList<ControlLecheAnimal>> ListByAnimalAsync(AnimalId idAnimal, DateOnly desde, DateOnly hasta, CancellationToken ct = default);
}
