using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Application.Ports.Output;

public interface IParametroLactanciaRepository : IRepository<ParametroLactancia, ParametroLactanciaId>
{
    Task<IReadOnlyList<ParametroLactancia>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default);
}
