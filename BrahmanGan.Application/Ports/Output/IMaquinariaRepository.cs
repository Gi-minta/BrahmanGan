using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Equipos;

namespace BrahmanGan.Application.Ports.Output;

public interface IMaquinariaRepository : IRepository<Maquinaria, MaquinariaId>
{
    Task<Maquinaria?> GetByCodigoAsync(string codigo, CancellationToken ct = default);
    Task<IReadOnlyList<Maquinaria>> ListActivasAsync(CancellationToken ct = default);
}

public interface IMantenimientoEquipoRepository : IRepository<MantenimientoEquipo, MantenimientoEquipoId>
{
    Task<IReadOnlyList<MantenimientoEquipo>> ListPorMaquinariaAsync(MaquinariaId idMaq, CancellationToken ct = default);
}
