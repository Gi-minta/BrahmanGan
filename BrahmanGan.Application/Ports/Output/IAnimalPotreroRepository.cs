using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Application.Ports.Output;

public interface IAnimalPotreroRepository : IRepository<AnimalPotrero, AnimalPotreroId>
{
    Task<AnimalPotrero?> GetVigenteByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default);
    Task<IReadOnlyList<AnimalPotrero>> ListByPotreroAsync(PotreroId idPotrero, CancellationToken ct = default);
}
