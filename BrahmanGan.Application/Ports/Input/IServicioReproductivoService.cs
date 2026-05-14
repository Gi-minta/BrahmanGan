using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// Fase 3
public interface IServicioReproductivoService
{
    Task<ServicioResponse> RegistrarMontaAsync(RegistrarMontaRequest req, CancellationToken ct = default);
    Task<ServicioResponse> RegistrarIaAsync(RegistrarIaRequest req, CancellationToken ct = default);
    Task ConfirmarAsync(int id, ConfirmarServicioRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ServicioResponse>> ListarPorHembraAsync(int idHembra, CancellationToken ct = default);
}
