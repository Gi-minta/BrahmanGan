using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Almacen;

namespace BrahmanGan.Application.Ports.Output;

public interface IInsumoRepository : IRepository<Insumo, InsumoId>
{
    Task<Insumo?> GetByCodigoAsync(string codigo, CancellationToken ct = default);
    Task<IReadOnlyList<Insumo>> ListActivosAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Insumo>> ListBajoMinimoAsync(CancellationToken ct = default);
}

public interface IKardexInsumoRepository : IRepository<KardexInsumo, KardexInsumoId>
{
    Task<IReadOnlyList<KardexInsumo>> ListPorInsumoAsync(InsumoId idInsumo, CancellationToken ct = default);
}
