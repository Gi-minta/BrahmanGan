using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Trazabilidad;

namespace BrahmanGan.Application.Ports.Output;

public interface IRegistroICARepository : IRepository<RegistroICA, RegistroICAId>
{
    Task<IReadOnlyList<RegistroICA>> ListPorAnimalAsync(AnimalId idAnimal, CancellationToken ct = default);
    Task<IReadOnlyList<RegistroICA>> ListProximosVencerAsync(DateOnly hoy, int diasUmbral, CancellationToken ct = default);
}
