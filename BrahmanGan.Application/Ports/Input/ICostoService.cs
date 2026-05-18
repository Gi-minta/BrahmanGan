using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// ===== Fase 7: Costos =====
public interface ICentroCostoService
{
    Task<CentroCostoResponse> CrearAsync(CrearCentroCostoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<CentroCostoResponse>> ListarAsync(CancellationToken ct = default);
}

public interface IGastoGeneralService
{
    Task<GastoGeneralResponse> CrearAsync(CrearGastoGeneralRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<GastoGeneralResponse>> ListarPorPeriodoAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default);
}

public interface IIngresoService
{
    Task<IngresoResponse> CrearAsync(CrearIngresoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<IngresoResponse>> ListarPorCentroAsync(int idCentro, CancellationToken ct = default);
}
