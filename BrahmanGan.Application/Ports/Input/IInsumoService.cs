using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// ===== Fase 7: Almacén =====
public interface IInsumoService
{
    Task<InsumoResponse> CrearAsync(CrearInsumoRequest req, CancellationToken ct = default);
    Task<InsumoResponse?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<InsumoResponse>> ListarAsync(CancellationToken ct = default);
    Task<IReadOnlyList<InsumoResponse>> ListarBajoMinimoAsync(CancellationToken ct = default);
    Task<KardexInsumoResponse> RegistrarMovimientoAsync(RegistrarMovimientoKardexRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<KardexInsumoResponse>> ListarKardexAsync(int idInsumo, CancellationToken ct = default);
}
