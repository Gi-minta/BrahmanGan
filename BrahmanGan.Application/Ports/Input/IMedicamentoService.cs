using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// Fase 4
public interface IMedicamentoService
{
    Task<MedicamentoResponse> CrearAsync(CrearMedicamentoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<MedicamentoResponse>> ListarAsync(CancellationToken ct = default);

    Task<ControlPreventivoResponse> CrearControlAsync(CrearControlPreventivoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ControlPreventivoResponse>> ListarControlesAsync(CancellationToken ct = default);
    Task<HistorialPreventivoResponse> AplicarControlAsync(AplicarControlPreventivoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<HistorialPreventivoResponse>> ListarHistorialPreventivoAsync(int idAnimal, CancellationToken ct = default);
}
