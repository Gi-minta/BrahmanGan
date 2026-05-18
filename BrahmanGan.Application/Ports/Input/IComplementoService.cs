using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IComplementoService
{
    Task<ComplementoResponse> RegistrarAsync(RegistrarComplementoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ComplementoResponse>> ListarPorTratamientoAsync(int idTratamiento, CancellationToken ct = default);
}
