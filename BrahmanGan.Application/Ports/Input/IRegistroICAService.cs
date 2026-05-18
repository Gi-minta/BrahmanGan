using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// ===== Fase 7: Trazabilidad =====
public interface IRegistroICAService
{
    Task<RegistroICAResponse> EmitirAsync(EmitirRegistroICARequest req, CancellationToken ct = default);
    Task<IReadOnlyList<RegistroICAResponse>> ListarPorAnimalAsync(int idAnimal, CancellationToken ct = default);
    Task<IReadOnlyList<RegistroICAResponse>> ListarProximosVencerAsync(int diasUmbral = 30, CancellationToken ct = default);
    Task CancelarAsync(int id, CancellationToken ct = default);
}
