using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// Fase 2
public interface IFincaService
{
    Task<FincaResponse> CrearAsync(CrearFincaRequest req, CancellationToken ct = default);
    Task<FincaResponse?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<FincaResponse>> ListarAsync(CancellationToken ct = default);
}
