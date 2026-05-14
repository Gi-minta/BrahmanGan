using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// Fase 4
public interface IMedicamentoService
{
    Task<MedicamentoResponse> CrearAsync(CrearMedicamentoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<MedicamentoResponse>> ListarAsync(CancellationToken ct = default);
}
