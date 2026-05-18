using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Application.UseCases;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Application.UseCases.Costos;

public sealed class CentroCostoService : ICentroCostoService
{
    private readonly ICentroCostoRepository _repo;
    private readonly IUnitOfWork _uow;
    public CentroCostoService(ICentroCostoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<CentroCostoResponse> CrearAsync(CrearCentroCostoRequest req, CancellationToken ct = default)
    {
        var c = CentroCosto.Crear(req.Codigo, req.Nombre,
            req.IdFinca.HasValue ? FincaId.From(req.IdFinca.Value) : null);
        await _repo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<IReadOnlyList<CentroCostoResponse>> ListarAsync(CancellationToken ct = default)
        => (await _repo.ListActivosAsync(ct)).Select(c => c.ToDto()).ToList();
}

public sealed class GastoGeneralService : IGastoGeneralService
{
    private readonly IGastoGeneralRepository _repo;
    private readonly IUnitOfWork _uow;
    public GastoGeneralService(IGastoGeneralRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<GastoGeneralResponse> CrearAsync(CrearGastoGeneralRequest req, CancellationToken ct = default)
    {
        var g = GastoGeneral.Crear(req.Fecha, req.Concepto, req.Valor,
            req.IdCentro.HasValue ? CentroCostoId.From(req.IdCentro.Value) : null,
            req.Proveedor, req.Comprobante);
        await _repo.AddAsync(g, ct);
        await _uow.SaveChangesAsync(ct);
        return g.ToDto();
    }

    public async Task<IReadOnlyList<GastoGeneralResponse>> ListarPorPeriodoAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default)
        => (await _repo.ListPorPeriodoAsync(desde, hasta, ct)).Select(g => g.ToDto()).ToList();
}

public sealed class IngresoService : IIngresoService
{
    private readonly IIngresoRepository _repo;
    private readonly IUnitOfWork _uow;
    public IngresoService(IIngresoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<IngresoResponse> CrearAsync(CrearIngresoRequest req, CancellationToken ct = default)
    {
        var i = Ingreso.Crear(req.Fecha, CentroCostoId.From(req.IdCentro), req.TipoIngreso, req.Valor,
            req.Concepto, req.Comprobante);
        await _repo.AddAsync(i, ct);
        await _uow.SaveChangesAsync(ct);
        return i.ToDto();
    }

    public async Task<IReadOnlyList<IngresoResponse>> ListarPorCentroAsync(int idCentro, CancellationToken ct = default)
        => (await _repo.ListPorCentroAsync(CentroCostoId.From(idCentro), ct)).Select(i => i.ToDto()).ToList();
}
