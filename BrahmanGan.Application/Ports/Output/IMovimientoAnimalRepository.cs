using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Application.Ports.Output;

public interface IMovimientoAnimalRepository : IRepository<MovimientoAnimal, MovimientoAnimalId>
{
    Task<IReadOnlyList<MovimientoAnimal>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default);
}
