using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Application.UseCases;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Trazabilidad;

namespace BrahmanGan.Application.UseCases.Trazabilidad;

public sealed class RegistroICAService : IRegistroICAService
{
    private readonly IRegistroICARepository _repo;
    private readonly IUnitOfWork _uow;
    public RegistroICAService(IRegistroICARepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<RegistroICAResponse> EmitirAsync(EmitirRegistroICARequest req, CancellationToken ct = default)
    {
        var r = RegistroICA.Emitir(AnimalId.From(req.IdAnimal), req.TipoDocumento, req.NumeroDocumento,
            req.FechaExpedicion, req.FechaVencimiento, req.EntidadEmisora,
            req.IdMunicipio.HasValue ? MunicipioId.From(req.IdMunicipio.Value) : null,
            req.UrlDocumento, req.Observaciones);
        await _repo.AddAsync(r, ct);
        await _uow.SaveChangesAsync(ct);
        return r.ToDto();
    }

    public async Task<IReadOnlyList<RegistroICAResponse>> ListarPorAnimalAsync(int idAnimal, CancellationToken ct = default)
        => (await _repo.ListPorAnimalAsync(AnimalId.From(idAnimal), ct)).Select(r => r.ToDto()).ToList();

    public async Task<IReadOnlyList<RegistroICAResponse>> ListarProximosVencerAsync(int diasUmbral = 30, CancellationToken ct = default)
    {
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        return (await _repo.ListProximosVencerAsync(hoy, diasUmbral, ct)).Select(r => r.ToDto()).ToList();
    }

    public async Task CancelarAsync(int id, CancellationToken ct = default)
    {
        var r = await _repo.GetByIdAsync(RegistroICAId.From(id), ct)
            ?? throw new DomainException("Registro ICA no encontrado");
        r.Cancelar();
        _repo.Update(r);
        await _uow.SaveChangesAsync(ct);
    }
}
