using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Application.UseCases.Animales;

public sealed class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _repo;
    private readonly IUnitOfWork _uow;

    public AnimalService(IAnimalRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<AnimalResponse> RegistrarAsync(CrearAnimalRequest req, CancellationToken ct = default)
    {
        if (await _repo.GetByCodigoAsync(req.Codigo, ct) is not null)
            throw new BusinessRuleException($"Ya existe un animal con código '{req.Codigo}'");

        var a = Animal.Registrar(
            req.Codigo, req.Sexo, RazaId.From(req.IdRaza), FincaId.From(req.IdFinca),
            req.Nombre, req.FechaNacimiento, req.PesoNacimiento,
            req.IdMadre is int m ? AnimalId.From(m) : null,
            req.IdPadre is int p ? AnimalId.From(p) : null,
            req.IdOrigen is int o ? OrigenId.From(o) : null,
            req.FechaIngreso, req.Observaciones);

        await _repo.AddAsync(a, ct);
        await _uow.SaveChangesAsync(ct);
        return a.ToDto();
    }

    public async Task<AnimalResponse?> ObtenerAsync(int id, CancellationToken ct = default)
        => (await _repo.GetByIdAsync(AnimalId.From(id), ct))?.ToDto();

    public async Task<IReadOnlyList<AnimalResponse>> ListarPorFincaAsync(int idFinca, CancellationToken ct = default)
        => (await _repo.ListByFincaAsync(FincaId.From(idFinca), ct)).Select(a => a.ToDto()).ToList();

    public async Task<IReadOnlyList<AnimalResponse>> ListarActivosAsync(CancellationToken ct = default)
        => (await _repo.ListActivosAsync(ct)).Select(a => a.ToDto()).ToList();

    public async Task CambiarEstadoAsync(int id, CambiarEstadoAnimalRequest req, CancellationToken ct = default)
    {
        var a = await _repo.GetByIdAsync(AnimalId.From(id), ct)
            ?? throw new EntityNotFoundException(nameof(Animal), id);
        a.CambiarEstado(req.Nuevo);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task TrasladarAsync(int id, TrasladarAnimalRequest req, CancellationToken ct = default)
    {
        var a = await _repo.GetByIdAsync(AnimalId.From(id), ct)
            ?? throw new EntityNotFoundException(nameof(Animal), id);
        a.Trasladar(FincaId.From(req.NuevaFinca));
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class RazaService : IRazaService
{
    private readonly IRazaRepository _repo;
    private readonly IUnitOfWork _uow;
    public RazaService(IRazaRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<RazaResponse> CrearAsync(CrearRazaRequest req, CancellationToken ct = default)
    {
        var r = Raza.Crear(req.Codigo, req.Nombre, req.Proposito);
        await _repo.AddAsync(r, ct);
        await _uow.SaveChangesAsync(ct);
        return r.ToDto();
    }

    public async Task<IReadOnlyList<RazaResponse>> ListarAsync(CancellationToken ct = default)
        => (await _repo.ListAllAsync(ct)).Select(r => r.ToDto()).ToList();
}

public sealed class PesajeService : IPesajeService
{
    private readonly IPesajeRepository _repo;
    private readonly IUnitOfWork _uow;
    public PesajeService(IPesajeRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<PesajeResponse> RegistrarAsync(RegistrarPesajeRequest req, CancellationToken ct = default)
    {
        var p = Pesaje.Registrar(AnimalId.From(req.IdAnimal), req.Fecha, req.PesoKg,
            req.CondicionCorporal, req.MetodoPesaje, req.Responsable);
        await _repo.AddAsync(p, ct);
        await _uow.SaveChangesAsync(ct);
        return p.ToDto();
    }

    public async Task<IReadOnlyList<PesajeResponse>> ListarPorAnimalAsync(int idAnimal, CancellationToken ct = default)
        => (await _repo.ListByAnimalAsync(AnimalId.From(idAnimal), ct)).Select(p => p.ToDto()).ToList();
}
