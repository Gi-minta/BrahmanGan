using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// ===== Pastoreo =====
public interface IPastoreoService
{
    Task<PlanPastoreoResponse> CrearPlanAsync(CrearPlanPastoreoRequest req, CancellationToken ct = default);
    Task<PlanPastoreoResponse?> ObtenerPlanAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PlanPastoreoResponse>> ListarPlanesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PlanPastoreoResponse>> ListarPorPotreroAsync(int idPotrero, CancellationToken ct = default);
    Task FinalizarPlanAsync(int id, DateOnly fechaFin, CancellationToken ct = default);
}
