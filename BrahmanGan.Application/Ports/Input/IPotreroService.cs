using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IPotreroService
{
    Task<PotreroResponse> CrearAsync(CrearPotreroRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<PotreroResponse>> ListarPorFincaAsync(int idFinca, CancellationToken ct = default);
}
