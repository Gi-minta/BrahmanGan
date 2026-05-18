using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Almacen;
using BrahmanGan.Domain.Modulos.Finca;
using FincaEntity = BrahmanGan.Domain.Modulos.Finca.Finca;

namespace BrahmanGan.Application.UseCases.Fincas;

public sealed class FincaService : IFincaService
{
    private readonly IFincaRepository _repo;
    private readonly IUnitOfWork _uow;
    public FincaService(IFincaRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<FincaResponse> CrearAsync(CrearFincaRequest req, CancellationToken ct = default)
    {
        var f = FincaEntity.Crear(req.Nombre,
            req.IdMunicipio is int m ? MunicipioId.From(m) : null,
            req.NIT, req.Propietario, req.Direccion, req.Telefono, req.Email, req.AreaHectareas);
        await _repo.AddAsync(f, ct);
        await _uow.SaveChangesAsync(ct);
        return f.ToDto();
    }

    public async Task<FincaResponse?> ObtenerAsync(int id, CancellationToken ct = default)
        => (await _repo.GetByIdAsync(FincaId.From(id), ct))?.ToDto();

    public async Task<IReadOnlyList<FincaResponse>> ListarAsync(CancellationToken ct = default)
        => (await _repo.ListAllAsync(ct)).Select(f => f.ToDto()).ToList();
}

public sealed class PotreroService : IPotreroService
{
    private readonly IPotreroRepository _repo;
    private readonly IGrupoManejoRepository _grupoRepo;
    private readonly IAnimalPotreroRepository _animalPotreroRepo;
    private readonly IAcumulacionInsumoPotreroRepository _acumRepo;
    private readonly IUnitOfWork _uow;

    public PotreroService(IPotreroRepository repo, IGrupoManejoRepository grupoRepo,
        IAnimalPotreroRepository animalPotreroRepo, IAcumulacionInsumoPotreroRepository acumRepo,
        IUnitOfWork uow)
    { _repo = repo; _grupoRepo = grupoRepo; _animalPotreroRepo = animalPotreroRepo; _acumRepo = acumRepo; _uow = uow; }

    public async Task<PotreroResponse> CrearAsync(CrearPotreroRequest req, CancellationToken ct = default)
    {
        var p = Potrero.Crear(FincaId.From(req.IdFinca), req.Codigo, req.Nombre, req.AreaHectareas, req.TipoPasto);
        await _repo.AddAsync(p, ct);
        await _uow.SaveChangesAsync(ct);
        return p.ToDto();
    }

    public async Task<IReadOnlyList<PotreroResponse>> ListarPorFincaAsync(int idFinca, CancellationToken ct = default)
        => (await _repo.ListByFincaAsync(FincaId.From(idFinca), ct)).Select(p => p.ToDto()).ToList();

    public async Task<GrupoManejoResponse> CrearGrupoAsync(CrearGrupoManejoRequest req, CancellationToken ct = default)
    {
        var g = GrupoManejo.Crear(req.Codigo, req.Nombre, req.Descripcion, req.TipoAnimal);
        await _grupoRepo.AddAsync(g, ct);
        await _uow.SaveChangesAsync(ct);
        return g.ToDto();
    }

    public async Task<IReadOnlyList<GrupoManejoResponse>> ListarGruposAsync(CancellationToken ct = default)
        => (await _grupoRepo.ListActivosAsync(ct)).Select(g => g.ToDto()).ToList();

    public async Task<AnimalPotreroResponse> AsignarAnimalAsync(AsignarAnimalPotreroRequest req, CancellationToken ct = default)
    {
        var ap = AnimalPotrero.Asignar(AnimalId.From(req.IdAnimal), PotreroId.From(req.IdPotrero),
            req.FechaIngreso, req.IdGrupo is int g ? GrupoManejoId.From(g) : null);
        await _animalPotreroRepo.AddAsync(ap, ct);
        await _uow.SaveChangesAsync(ct);
        return ap.ToDto();
    }

    public async Task<AnimalPotreroResponse> CerrarAsignacionAsync(int id, CerrarAnimalPotreroRequest req, CancellationToken ct = default)
    {
        var ap = await _animalPotreroRepo.GetByIdAsync(AnimalPotreroId.From(id), ct)
            ?? throw new EntityNotFoundException(nameof(AnimalPotrero), id);
        ap.Cerrar(req.FechaSalida);
        await _uow.SaveChangesAsync(ct);
        return ap.ToDto();
    }

    public async Task<IReadOnlyList<AnimalPotreroResponse>> ListarAnimalesPorPotreroAsync(int idPotrero, CancellationToken ct = default)
        => (await _animalPotreroRepo.ListByPotreroAsync(PotreroId.From(idPotrero), ct)).Select(ap => ap.ToDto()).ToList();

    public async Task<AcumulacionInsumoPotreroResponse> RegistrarAcumulacionAsync(RegistrarAcumulacionInsumoRequest req, CancellationToken ct = default)
    {
        var a = AcumulacionInsumoPotrero.Registrar(PotreroId.From(req.IdPotrero), InsumoId.From(req.IdInsumo),
            req.Fecha, req.Cantidad, req.CostoUnitario);
        await _acumRepo.AddAsync(a, ct);
        await _uow.SaveChangesAsync(ct);
        return a.ToDto();
    }

    public async Task<IReadOnlyList<AcumulacionInsumoPotreroResponse>> ListarAcumulacionesPorPotreroAsync(int idPotrero, CancellationToken ct = default)
        => (await _acumRepo.ListByPotreroAsync(PotreroId.From(idPotrero), ct)).Select(a => a.ToDto()).ToList();
}
