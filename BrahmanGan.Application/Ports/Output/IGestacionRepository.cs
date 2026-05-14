using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Application.Ports.Output;

public interface IGestacionRepository : IRepository<Gestacion, GestacionId>
{
    Task<Gestacion?> GetEnCursoByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default);
}
