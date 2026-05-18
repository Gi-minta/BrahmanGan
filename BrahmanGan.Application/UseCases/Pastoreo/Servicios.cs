using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Pastoreo;

namespace BrahmanGan.Application.UseCases.Pastoreo;

public sealed class PastoreoService : IPastoreoService
{
    private readonly IPlanPastoreoRepository _repo;
    private readonly IUnitOfWork _uow;

    public PastoreoService(IPlanPastoreoRepository repo, IUnitOfWork uow)
    {
        _repo = repo; _uow = uow;
    }

    public async Task<PlanPastoreoResponse> CrearPlanAsync(CrearPlanPastoreoRequest req, CancellationToken ct = default)
    {
        var plan = PlanPastoreo.Crear(PotreroId.From(req.IdPotrero), req.FechaInicio,
            req.FechaFin, req.NumAnimales, req.CapacidadCarga, req.Observaciones);
        await _repo.AddAsync(plan, ct);
        await _uow.SaveChangesAsync(ct);
        return plan.ToDto();
    }

    public async Task<PlanPastoreoResponse?> ObtenerPlanAsync(int id, CancellationToken ct = default)
    {
        var plan = await _repo.GetByIdAsync(PlanPastoreoId.From(id), ct);
        return plan?.ToDto();
    }

    public async Task<IReadOnlyList<PlanPastoreoResponse>> ListarPlanesAsync(CancellationToken ct = default)
        => (await _repo.ListActivosAsync(ct)).Select(p => p.ToDto()).ToList();

    public async Task<IReadOnlyList<PlanPastoreoResponse>> ListarPorPotreroAsync(int idPotrero, CancellationToken ct = default)
        => (await _repo.ListPorPotreroAsync(PotreroId.From(idPotrero), ct)).Select(p => p.ToDto()).ToList();

    public async Task FinalizarPlanAsync(int id, DateOnly fechaFin, CancellationToken ct = default)
    {
        var plan = await _repo.GetByIdAsync(PlanPastoreoId.From(id), ct)
            ?? throw new DomainException("Plan de pastoreo no encontrado");
        plan.Finalizar(fechaFin);
        _repo.Update(plan);
        await _uow.SaveChangesAsync(ct);
    }
}
