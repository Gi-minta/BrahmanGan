using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Application.UseCases.Leche;

public sealed class ControlLecheService : IControlLecheService
{
    private readonly IControlLecheAnimalRepository _repo;
    private readonly IParametroLactanciaRepository _paramRepo;
    private readonly ICalidadLecheRepository _calidadRepo;
    private readonly IUnitOfWork _uow;

    public ControlLecheService(IControlLecheAnimalRepository repo, IParametroLactanciaRepository paramRepo,
        ICalidadLecheRepository calidadRepo, IUnitOfWork uow)
    { _repo = repo; _paramRepo = paramRepo; _calidadRepo = calidadRepo; _uow = uow; }

    public async Task<ControlLecheResponse> RegistrarAsync(RegistrarControlLecheRequest req, CancellationToken ct = default)
    {
        var c = ControlLecheAnimal.Registrar(AnimalId.From(req.IdAnimal), req.Fecha, req.Maniana, req.Tarde, req.Noche, req.Ordeno);
        await _repo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<IReadOnlyList<ControlLecheResponse>> ListarPorAnimalAsync(int idAnimal, DateOnly desde, DateOnly hasta, CancellationToken ct = default)
        => (await _repo.ListByAnimalAsync(AnimalId.From(idAnimal), desde, hasta, ct)).Select(c => c.ToDto()).ToList();

    public async Task<ParametroLactanciaResponse> IniciarLactanciaAsync(IniciarParametroLactanciaRequest req, CancellationToken ct = default)
    {
        var p = ParametroLactancia.Iniciar(AnimalId.From(req.IdAnimal), req.NumeroParto, req.FechaInicio);
        await _paramRepo.AddAsync(p, ct);
        await _uow.SaveChangesAsync(ct);
        return p.ToDto();
    }

    public async Task<ParametroLactanciaResponse> CerrarLactanciaAsync(int id, CerrarParametroLactanciaRequest req, CancellationToken ct = default)
    {
        var p = await _paramRepo.GetByIdAsync(ParametroLactanciaId.From(id), ct)
            ?? throw new EntityNotFoundException(nameof(ParametroLactancia), id);
        p.Cerrar(req.FechaFin, req.LitrosTotales);
        await _uow.SaveChangesAsync(ct);
        return p.ToDto();
    }

    public async Task<IReadOnlyList<ParametroLactanciaResponse>> ListarLactanciasPorAnimalAsync(int idAnimal, CancellationToken ct = default)
        => (await _paramRepo.ListByAnimalAsync(AnimalId.From(idAnimal), ct)).Select(p => p.ToDto()).ToList();

    public async Task<CalidadLecheResponse> RegistrarCalidadAsync(RegistrarCalidadLecheRequest req, CancellationToken ct = default)
    {
        var c = CalidadLeche.Registrar(req.Fecha, req.IdAnimal is int a ? AnimalId.From(a) : null,
            req.CelSomaticas, req.GrasaPct, req.ProteinaPct, req.LactozaPct, req.UreaMgDL,
            req.Laboratorio, req.Resultado, req.Observaciones);
        await _calidadRepo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<IReadOnlyList<CalidadLecheResponse>> ListarCalidadPorFechaAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default)
        => (await _calidadRepo.ListPorPeriodoAsync(desde, hasta, ct)).Select(c => c.ToDto()).ToList();
}

public sealed class ProduccionLecheService : IProduccionLecheService
{
    private readonly IProduccionLecheRepository _repo;
    private readonly IUnitOfWork _uow;
    public ProduccionLecheService(IProduccionLecheRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<ProduccionLecheResponse> RegistrarAsync(RegistrarProduccionLecheRequest req, CancellationToken ct = default)
    {
        if (await _repo.GetByFincaFechaAsync(FincaId.From(req.IdFinca), req.Fecha, ct) is not null)
            throw new BusinessRuleException("Ya existe producción registrada para esa finca y fecha");
        var p = ProduccionLeche.Registrar(FincaId.From(req.IdFinca), req.Fecha, req.TotalLitros,
            req.Vendidos, req.Autoconsumo, req.Merma);
        await _repo.AddAsync(p, ct);
        await _uow.SaveChangesAsync(ct);
        return p.ToDto();
    }
}

public sealed class VentaLecheService : IVentaLecheService
{
    private readonly IVentaLecheRepository _repo;
    private readonly IUnitOfWork _uow;
    public VentaLecheService(IVentaLecheRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<VentaLecheResponse> RegistrarAsync(RegistrarVentaLecheRequest req, CancellationToken ct = default)
    {
        var v = VentaLeche.Registrar(req.Fecha, ClienteId.From(req.IdCliente), req.Litros, req.PrecioLitro,
            req.IdContrato is int c ? ContratoId.From(c) : null, req.Factura);
        await _repo.AddAsync(v, ct);
        await _uow.SaveChangesAsync(ct);
        return v.ToDto();
    }
}
