using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Alimentacion;

namespace BrahmanGan.Application.UseCases.Alimentacion;

public sealed class AlimentacionService : IAlimentacionService
{
    private readonly IPlanAlimentacionRepository _planRepo;
    private readonly IDetallePlanAlimentacionRepository _detalleRepo;
    private readonly IUnitOfWork _uow;

    public AlimentacionService(IPlanAlimentacionRepository planRepo,
        IDetallePlanAlimentacionRepository detalleRepo, IUnitOfWork uow)
    {
        _planRepo = planRepo; _detalleRepo = detalleRepo; _uow = uow;
    }

    public async Task<PlanAlimentacionResponse> CrearPlanAsync(CrearPlanAlimentacionRequest req, CancellationToken ct = default)
    {
        var plan = PlanAlimentacion.Crear(FincaId.From(req.IdFinca), req.Nombre,
            req.FechaInicio, req.FechaFin, req.Observaciones);
        await _planRepo.AddAsync(plan, ct);
        await _uow.SaveChangesAsync(ct);
        return plan.ToDto();
    }

    public async Task<PlanAlimentacionResponse?> ObtenerPlanAsync(int id, CancellationToken ct = default)
    {
        var plan = await _planRepo.GetByIdAsync(PlanAlimentacionId.From(id), ct);
        return plan?.ToDto();
    }

    public async Task<IReadOnlyList<PlanAlimentacionResponse>> ListarPlanesAsync(CancellationToken ct = default)
        => (await _planRepo.ListActivosAsync(ct)).Select(p => p.ToDto()).ToList();

    public async Task<IReadOnlyList<PlanAlimentacionResponse>> ListarPorFincaAsync(int idFinca, CancellationToken ct = default)
        => (await _planRepo.ListPorFincaAsync(FincaId.From(idFinca), ct)).Select(p => p.ToDto()).ToList();

    public async Task DesactivarPlanAsync(int id, CancellationToken ct = default)
    {
        var plan = await _planRepo.GetByIdAsync(PlanAlimentacionId.From(id), ct)
            ?? throw new DomainException("Plan de alimentación no encontrado");
        plan.Desactivar();
        _planRepo.Update(plan);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task<DetallePlanAlimentacionResponse> AgregarDetalleAsync(AgregarDetallePlanRequest req, CancellationToken ct = default)
    {
        var plan = await _planRepo.GetByIdAsync(PlanAlimentacionId.From(req.IdPlan), ct)
            ?? throw new DomainException("Plan de alimentación no encontrado");
        var insumoId = req.IdInsumo.HasValue ? InsumoId.From(req.IdInsumo.Value) : null;
        var detalle = DetallePlanAlimentacion.Crear(plan.Id, req.Alimento,
            req.CantidadDiaria, req.UnidadMedida, insumoId, req.Observaciones);
        await _detalleRepo.AddAsync(detalle, ct);
        await _uow.SaveChangesAsync(ct);
        return detalle.ToDto();
    }

    public async Task<IReadOnlyList<DetallePlanAlimentacionResponse>> ListarDetallesAsync(int idPlan, CancellationToken ct = default)
        => (await _detalleRepo.ListPorPlanAsync(PlanAlimentacionId.From(idPlan), ct)).Select(d => d.ToDto()).ToList();
}
