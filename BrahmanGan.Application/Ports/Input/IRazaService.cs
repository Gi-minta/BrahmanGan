using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IRazaService
{
    Task<RazaResponse> CrearAsync(CrearRazaRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<RazaResponse>> ListarAsync(CancellationToken ct = default);
}
