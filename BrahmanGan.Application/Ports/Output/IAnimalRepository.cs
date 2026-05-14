using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Application.Ports.Output;

// Fase 1
public interface IAnimalRepository : IRepository<Animal, AnimalId>
{
    Task<Animal?> GetByCodigoAsync(string codigo, CancellationToken ct = default);
    Task<IReadOnlyList<Animal>> ListByFincaAsync(FincaId idFinca, CancellationToken ct = default);
    Task<IReadOnlyList<Animal>> ListActivosAsync(CancellationToken ct = default);
}
