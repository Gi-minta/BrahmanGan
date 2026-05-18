using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Application.UseCases;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sostenibilidad;

namespace BrahmanGan.Application.UseCases.Sostenibilidad;

public sealed class SostenibilidadService : ISostenibilidadService
{
    private readonly ICapturaCarbonoRepository _carbonoRepo;
    private readonly IConsumoAguaRepository _aguaRepo;
    private readonly IEventoMedioambientalRepository _eventoRepo;
    private readonly IUnitOfWork _uow;

    public SostenibilidadService(
        ICapturaCarbonoRepository carbonoRepo,
        IConsumoAguaRepository aguaRepo,
        IEventoMedioambientalRepository eventoRepo,
        IUnitOfWork uow)
    {
        _carbonoRepo = carbonoRepo; _aguaRepo = aguaRepo;
        _eventoRepo = eventoRepo; _uow = uow;
    }

    public async Task<CapturaCarbonoResponse> RegistrarCapturaAsync(RegistrarCapturaCarbonoRequest req, CancellationToken ct = default)
    {
        var c = CapturaCarbono.Registrar(FincaId.From(req.IdFinca), req.Anio, req.Mes,
            req.EmisionesGanadoTCO2, req.CapturaForestal, req.Certificacion, req.Observaciones);
        await _carbonoRepo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<IReadOnlyList<CapturaCarbonoResponse>> ListarCapturasPorFincaAsync(int idFinca, CancellationToken ct = default)
        => (await _carbonoRepo.ListPorFincaAsync(FincaId.From(idFinca), ct)).Select(c => c.ToDto()).ToList();

    public async Task<ConsumoAguaResponse> RegistrarConsumoAguaAsync(RegistrarConsumoAguaRequest req, CancellationToken ct = default)
    {
        var c = ConsumoAgua.Registrar(FincaId.From(req.IdFinca), req.Fecha, req.VolumenM3,
            req.IdPotrero.HasValue ? PotreroId.From(req.IdPotrero.Value) : null,
            req.FuenteAgua, req.NumAnimales, req.Observaciones);
        await _aguaRepo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<IReadOnlyList<ConsumoAguaResponse>> ListarConsumoAguaPorFincaAsync(int idFinca, CancellationToken ct = default)
        => (await _aguaRepo.ListPorFincaAsync(FincaId.From(idFinca), ct)).Select(c => c.ToDto()).ToList();

    public async Task<EventoMedioambientalResponse> RegistrarEventoAsync(RegistrarEventoMedioambientalRequest req, CancellationToken ct = default)
    {
        var e = EventoMedioambiental.Registrar(FincaId.From(req.IdFinca), req.Fecha, req.TipoEvento,
            req.Descripcion, req.Intensidad, req.PrecipitacionMM, req.TempMaxC, req.TempMinC, req.ImpactoEstimado);
        await _eventoRepo.AddAsync(e, ct);
        await _uow.SaveChangesAsync(ct);
        return e.ToDto();
    }

    public async Task<IReadOnlyList<EventoMedioambientalResponse>> ListarEventosPorFincaAsync(int idFinca, CancellationToken ct = default)
        => (await _eventoRepo.ListPorFincaAsync(FincaId.From(idFinca), ct)).Select(e => e.ToDto()).ToList();
}
