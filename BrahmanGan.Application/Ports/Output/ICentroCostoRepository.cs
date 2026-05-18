using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Application.Ports.Output;

public interface ICentroCostoRepository : IRepository<CentroCosto, CentroCostoId>
{
    Task<IReadOnlyList<CentroCosto>> ListActivosAsync(CancellationToken ct = default);
}

public interface IGastoGeneralRepository : IRepository<GastoGeneral, GastoGeneralId>
{
    Task<IReadOnlyList<GastoGeneral>> ListPorPeriodoAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default);
}

public interface IIngresoRepository : IRepository<Ingreso, IngresoId>
{
    Task<IReadOnlyList<Ingreso>> ListPorCentroAsync(CentroCostoId idCentro, CancellationToken ct = default);
}
