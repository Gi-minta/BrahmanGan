using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
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
    private readonly IUnitOfWork _uow;
    public PotreroService(IPotreroRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<PotreroResponse> CrearAsync(CrearPotreroRequest req, CancellationToken ct = default)
    {
        var p = Potrero.Crear(FincaId.From(req.IdFinca), req.Codigo, req.Nombre, req.AreaHectareas, req.TipoPasto);
        await _repo.AddAsync(p, ct);
        await _uow.SaveChangesAsync(ct);
        return p.ToDto();
    }

    public async Task<IReadOnlyList<PotreroResponse>> ListarPorFincaAsync(int idFinca, CancellationToken ct = default)
        => (await _repo.ListByFincaAsync(FincaId.From(idFinca), ct)).Select(p => p.ToDto()).ToList();
}
