using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Application.Ports.Output;

public interface IPesajeRepository : IRepository<Pesaje, PesajeId>
{
    Task<IReadOnlyList<Pesaje>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default);
}
