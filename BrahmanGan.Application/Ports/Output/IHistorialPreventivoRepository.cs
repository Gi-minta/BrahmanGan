using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Application.Ports.Output;

public interface IHistorialPreventivoRepository : IRepository<HistorialPreventivo, HistorialPreventivoId>
{
    Task<IReadOnlyList<HistorialPreventivo>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default);
}
