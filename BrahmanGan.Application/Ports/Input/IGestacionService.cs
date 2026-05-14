using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IGestacionService
{
    Task<GestacionResponse> IniciarAsync(IniciarGestacionRequest req, CancellationToken ct = default);
    Task RegistrarPartoAsync(int id, RegistrarPartoRequest req, CancellationToken ct = default);
    Task RegistrarAbortoAsync(int id, RegistrarAbortoRequest req, CancellationToken ct = default);
    Task<GestacionResponse?> ObtenerAsync(int id, CancellationToken ct = default);
}
