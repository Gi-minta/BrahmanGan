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
    private readonly IUnitOfWork _uow;
    public ControlLecheService(IControlLecheAnimalRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<ControlLecheResponse> RegistrarAsync(RegistrarControlLecheRequest req, CancellationToken ct = default)
    {
        var c = ControlLecheAnimal.Registrar(AnimalId.From(req.IdAnimal), req.Fecha, req.Maniana, req.Tarde, req.Noche, req.Ordeno);
        await _repo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<IReadOnlyList<ControlLecheResponse>> ListarPorAnimalAsync(int idAnimal, DateOnly desde, DateOnly hasta, CancellationToken ct = default)
        => (await _repo.ListByAnimalAsync(AnimalId.From(idAnimal), desde, hasta, ct)).Select(c => c.ToDto()).ToList();
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
