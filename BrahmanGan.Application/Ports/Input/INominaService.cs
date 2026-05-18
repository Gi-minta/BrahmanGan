using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// ===== Fase 8: Nómina =====
public interface ITrabajadorService
{
    Task<TrabajadorResponse> ContratarAsync(ContratarTrabajadorRequest req, CancellationToken ct = default);
    Task<TrabajadorResponse?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<TrabajadorResponse>> ListarAsync(CancellationToken ct = default);
    Task<PagoJornalResponse> RegistrarPagoAsync(RegistrarPagoJornalRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<PagoJornalResponse>> ListarPagosAsync(int idTrabajador, CancellationToken ct = default);
}
