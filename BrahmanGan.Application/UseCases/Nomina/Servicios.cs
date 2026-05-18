using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Application.UseCases;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Nomina;

namespace BrahmanGan.Application.UseCases.Nomina;

public sealed class TrabajadorService : ITrabajadorService
{
    private readonly ITrabajadorRepository _repo;
    private readonly IPagoJornalRepository _pagoRepo;
    private readonly IUnitOfWork _uow;
    public TrabajadorService(ITrabajadorRepository repo, IPagoJornalRepository pagoRepo, IUnitOfWork uow)
    {
        _repo = repo; _pagoRepo = pagoRepo; _uow = uow;
    }

    public async Task<TrabajadorResponse> ContratarAsync(ContratarTrabajadorRequest req, CancellationToken ct = default)
    {
        var t = Trabajador.Contratar(req.Cedula, req.Nombres, req.Apellidos, req.FechaIngreso,
            req.Cargo, req.SalarioBase, req.TipoContrato);
        await _repo.AddAsync(t, ct);
        await _uow.SaveChangesAsync(ct);
        return t.ToDto();
    }

    public async Task<TrabajadorResponse?> ObtenerAsync(int id, CancellationToken ct = default)
    {
        var t = await _repo.GetByIdAsync(TrabajadorId.From(id), ct);
        return t?.ToDto();
    }

    public async Task<IReadOnlyList<TrabajadorResponse>> ListarAsync(CancellationToken ct = default)
        => (await _repo.ListActivosAsync(ct)).Select(t => t.ToDto()).ToList();

    public async Task<PagoJornalResponse> RegistrarPagoAsync(RegistrarPagoJornalRequest req, CancellationToken ct = default)
    {
        var trabajador = await _repo.GetByIdAsync(TrabajadorId.From(req.IdTrabajador), ct)
            ?? throw new DomainException("Trabajador no encontrado");
        var pago = PagoJornal.Registrar(trabajador.Id, req.Fecha, req.ValorJornal,
            req.HorasTrabajadas, req.Concepto,
            req.IdCentro.HasValue ? CentroCostoId.From(req.IdCentro.Value) : null);
        await _pagoRepo.AddAsync(pago, ct);
        await _uow.SaveChangesAsync(ct);
        return pago.ToDto();
    }

    public async Task<IReadOnlyList<PagoJornalResponse>> ListarPagosAsync(int idTrabajador, CancellationToken ct = default)
        => (await _pagoRepo.ListPorTrabajadorAsync(TrabajadorId.From(idTrabajador), ct)).Select(p => p.ToDto()).ToList();
}
