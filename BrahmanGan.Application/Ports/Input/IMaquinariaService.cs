using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// ===== Fase 7: Equipos =====
public interface IMaquinariaService
{
    Task<MaquinariaResponse> CrearAsync(CrearMaquinariaRequest req, CancellationToken ct = default);
    Task<MaquinariaResponse?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<MaquinariaResponse>> ListarAsync(CancellationToken ct = default);
    Task<MantenimientoEquipoResponse> RegistrarMantenimientoAsync(RegistrarMantenimientoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<MantenimientoEquipoResponse>> ListarMantenimientosAsync(int idMaquinaria, CancellationToken ct = default);
}
