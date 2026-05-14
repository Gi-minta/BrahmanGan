using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// Fase 6
public interface IClienteService
{
    Task<ClienteResponse> CrearAsync(CrearClienteRequest req, CancellationToken ct = default);
    Task<ClienteResponse?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ClienteResponse>> ListarAsync(CancellationToken ct = default);
}
