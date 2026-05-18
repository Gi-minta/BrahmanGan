using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Application.Ports.Output;

public interface IPedigriRepository : IRepository<Pedigri, PedigriId>
{
    Task<Pedigri?> GetByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default);
}
