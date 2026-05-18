using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Application.UseCases.Reproduccion;

public sealed class ServicioReproductivoService : IServicioReproductivoService
{
    private readonly IServicioRepository _repo;
    private readonly ISemenRepository _semenRepo;
    private readonly INacimientoRepository _nacRepo;
    private readonly IUnitOfWork _uow;

    public ServicioReproductivoService(IServicioRepository repo, ISemenRepository semenRepo,
        INacimientoRepository nacRepo, IUnitOfWork uow)
    { _repo = repo; _semenRepo = semenRepo; _nacRepo = nacRepo; _uow = uow; }

    public async Task<ServicioResponse> RegistrarMontaAsync(RegistrarMontaRequest req, CancellationToken ct = default)
    {
        var s = Servicio.RegistrarMonta(AnimalId.From(req.IdHembra), AnimalId.From(req.IdToro), req.Fecha, req.Responsable);
        await _repo.AddAsync(s, ct);
        await _uow.SaveChangesAsync(ct);
        return s.ToDto();
    }

    public async Task<ServicioResponse> RegistrarIaAsync(RegistrarIaRequest req, CancellationToken ct = default)
    {
        var s = Servicio.RegistrarIA(AnimalId.From(req.IdHembra), SemenId.From(req.IdSemen), req.Fecha, req.Responsable);
        await _repo.AddAsync(s, ct);
        await _uow.SaveChangesAsync(ct);
        return s.ToDto();
    }

    public async Task ConfirmarAsync(int id, ConfirmarServicioRequest req, CancellationToken ct = default)
    {
        var s = await _repo.GetByIdAsync(ServicioId.From(id), ct)
            ?? throw new EntityNotFoundException(nameof(Servicio), id);
        s.ConfirmarResultado(req.Preniada, req.FechaConfirmacion);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<ServicioResponse>> ListarPorHembraAsync(int idHembra, CancellationToken ct = default)
        => (await _repo.ListByHembraAsync(AnimalId.From(idHembra), ct)).Select(s => s.ToDto()).ToList();

    public async Task<SemenResponse> CrearSemenAsync(CrearSemenRequest req, CancellationToken ct = default)
    {
        if (await _semenRepo.GetByCodigoAsync(req.Codigo, ct) is not null)
            throw new BusinessRuleException($"Ya existe semen con código '{req.Codigo}'");
        var semen = Semen.Crear(req.Codigo, req.NombreToro,
            req.IdRaza is int r ? RazaId.From(r) : null, req.Casa, req.StockInicial);
        await _semenRepo.AddAsync(semen, ct);
        await _uow.SaveChangesAsync(ct);
        return semen.ToDto();
    }

    public async Task<SemenResponse?> ObtenerSemenAsync(int id, CancellationToken ct = default)
        => (await _semenRepo.GetByIdAsync(SemenId.From(id), ct))?.ToDto();

    public async Task<IReadOnlyList<SemenResponse>> ListarSemenAsync(CancellationToken ct = default)
        => (await _semenRepo.ListActivosAsync(ct)).Select(s => s.ToDto()).ToList();

    public async Task<SemenResponse> AjustarStockSemenAsync(AjustarStockSemenRequest req, CancellationToken ct = default)
    {
        var semen = await _semenRepo.GetByIdAsync(SemenId.From(req.IdSemen), ct)
            ?? throw new EntityNotFoundException(nameof(Semen), req.IdSemen);
        if (req.Dosis > 0) semen.IngresarStock(req.Dosis);
        else semen.ConsumirDosis(-req.Dosis);
        await _uow.SaveChangesAsync(ct);
        return semen.ToDto();
    }

    public async Task<NacimientoResponse?> ObtenerNacimientoAsync(int id, CancellationToken ct = default)
        => (await _nacRepo.GetByIdAsync(NacimientoId.From(id), ct))?.ToDto();

    public async Task<IReadOnlyList<NacimientoResponse>> ListarNacimientosPorGestacionAsync(int idGestacion, CancellationToken ct = default)
        => (await _nacRepo.ListByGestacionAsync(GestacionId.From(idGestacion), ct)).Select(n => n.ToDto()).ToList();
}

public sealed class GestacionService : IGestacionService
{
    private readonly IGestacionRepository _repo;
    private readonly IUnitOfWork _uow;
    public GestacionService(IGestacionRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<GestacionResponse> IniciarAsync(IniciarGestacionRequest req, CancellationToken ct = default)
    {
        if (await _repo.GetEnCursoByAnimalAsync(AnimalId.From(req.IdAnimal), ct) is not null)
            throw new BusinessRuleException("El animal ya tiene una gestación EN_CURSO");
        var g = Gestacion.Iniciar(AnimalId.From(req.IdAnimal), req.FechaInicio,
            req.IdServicio is int s ? ServicioId.From(s) : null, req.Observaciones);
        await _repo.AddAsync(g, ct);
        await _uow.SaveChangesAsync(ct);
        return g.ToDto();
    }

    public async Task RegistrarPartoAsync(int id, RegistrarPartoRequest req, CancellationToken ct = default)
    {
        var g = await _repo.GetByIdAsync(GestacionId.From(id), ct)
            ?? throw new EntityNotFoundException(nameof(Gestacion), id);
        g.RegistrarParto(req.FechaPartoReal);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task RegistrarAbortoAsync(int id, RegistrarAbortoRequest req, CancellationToken ct = default)
    {
        var g = await _repo.GetByIdAsync(GestacionId.From(id), ct)
            ?? throw new EntityNotFoundException(nameof(Gestacion), id);
        g.RegistrarAborto(req.Fecha, req.Motivo);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task<GestacionResponse?> ObtenerAsync(int id, CancellationToken ct = default)
        => (await _repo.GetByIdAsync(GestacionId.From(id), ct))?.ToDto();
}
