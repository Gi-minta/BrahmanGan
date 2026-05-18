using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// ===== Alimentación =====
public interface IAlimentacionService
{
    Task<PlanAlimentacionResponse> CrearPlanAsync(CrearPlanAlimentacionRequest req, CancellationToken ct = default);
    Task<PlanAlimentacionResponse?> ObtenerPlanAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PlanAlimentacionResponse>> ListarPlanesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PlanAlimentacionResponse>> ListarPorFincaAsync(int idFinca, CancellationToken ct = default);
    Task DesactivarPlanAsync(int id, CancellationToken ct = default);

    Task<DetallePlanAlimentacionResponse> AgregarDetalleAsync(AgregarDetallePlanRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<DetallePlanAlimentacionResponse>> ListarDetallesAsync(int idPlan, CancellationToken ct = default);
}
