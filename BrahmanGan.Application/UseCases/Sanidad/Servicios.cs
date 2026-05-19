using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Application.UseCases.Sanidad;

public sealed class MedicamentoService : IMedicamentoService
{
    private readonly IMedicamentoRepository _repo;
    private readonly IControlPreventivoRepository _controlRepo;
    private readonly IHistorialPreventivoRepository _histRepo;
    private readonly IUnitOfWork _uow;

    public MedicamentoService(IMedicamentoRepository repo, IControlPreventivoRepository controlRepo,
        IHistorialPreventivoRepository histRepo, IUnitOfWork uow)
    {
        _repo = repo; _controlRepo = controlRepo; _histRepo = histRepo; _uow = uow;
    }

    public async Task<MedicamentoResponse> CrearAsync(CrearMedicamentoRequest req, CancellationToken ct = default)
    {
        var m = Medicamento.Crear(req.Codigo, req.Nombre, req.Principio, req.TipoUso, req.Unidad,
            req.PrecioUnitario, req.TiempoCarne, req.TiempoLeche);
        await _repo.AddAsync(m, ct);
        await _uow.SaveChangesAsync(ct);
        return m.ToDto();
    }

    public async Task<IReadOnlyList<MedicamentoResponse>> ListarAsync(CancellationToken ct = default)
        => (await _repo.ListAllAsync(ct)).Select(m => m.ToDto()).ToList();

    public async Task<ControlPreventivoResponse> CrearControlAsync(CrearControlPreventivoRequest req, CancellationToken ct = default)
    {
        var c = ControlPreventivo.Crear(req.Nombre, req.Periodicidad, req.Descripcion);
        await _controlRepo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<IReadOnlyList<ControlPreventivoResponse>> ListarControlesAsync(CancellationToken ct = default)
        => (await _controlRepo.ListAllAsync(ct)).Select(c => c.ToDto()).ToList();

    public async Task<HistorialPreventivoResponse> AplicarControlAsync(AplicarControlPreventivoRequest req, CancellationToken ct = default)
    {
        var h = HistorialPreventivo.Aplicar(AnimalId.From(req.IdAnimal), ControlPreventivoId.From(req.IdControl),
            req.Fecha, req.IdMedicamento is int m ? MedicamentoId.From(m) : null,
            req.Dosis, req.Responsable, req.ProximaFecha);
        await _histRepo.AddAsync(h, ct);
        await _uow.SaveChangesAsync(ct);
        return h.ToDto();
    }

    public async Task<IReadOnlyList<HistorialPreventivoResponse>> ListarHistorialPreventivoAsync(int idAnimal, CancellationToken ct = default)
        => (await _histRepo.ListByAnimalAsync(AnimalId.From(idAnimal), ct)).Select(h => h.ToDto()).ToList();
}

public sealed class VacunacionService : IVacunacionService
{
    private readonly IHistorialVacunacionRepository _repo;
    private readonly IHistorialDesparasitacionRepository _despaRepo;
    private readonly IUnitOfWork _uow;

    public VacunacionService(IHistorialVacunacionRepository repo, IHistorialDesparasitacionRepository despaRepo,
        IUnitOfWork uow) { _repo = repo; _despaRepo = despaRepo; _uow = uow; }

    public async Task<VacunacionResponse> AplicarAsync(AplicarVacunaRequest req, CancellationToken ct = default)
    {
        var v = HistorialVacunacion.Aplicar(AnimalId.From(req.IdAnimal), MedicamentoId.From(req.IdMedicamento),
            req.Fecha, req.Dosis, req.Lote, req.Responsable, req.ProximaFecha);
        await _repo.AddAsync(v, ct);
        await _uow.SaveChangesAsync(ct);
        return v.ToDto();
    }

    public async Task<IReadOnlyList<VacunacionResponse>> ListarPorAnimalAsync(int idAnimal, CancellationToken ct = default)
        => (await _repo.ListByAnimalAsync(AnimalId.From(idAnimal), ct)).Select(v => v.ToDto()).ToList();

    public async Task<IReadOnlyList<VacunacionResponse>> ListarAlertasAsync(int diasUmbral = 7, CancellationToken ct = default)
        => (await _repo.ListConAlertaAsync(diasUmbral, ct)).Select(v => v.ToDto()).ToList();

    public async Task<DesparasitacionResponse> AplicarDesparasitacionAsync(AplicarDesparasitacionRequest req, CancellationToken ct = default)
    {
        var d = HistorialDesparasitacion.Aplicar(AnimalId.From(req.IdAnimal), MedicamentoId.From(req.IdMedicamento),
            req.Fecha, req.Dosis, req.TipoParasito, req.ProximaFecha);
        await _despaRepo.AddAsync(d, ct);
        await _uow.SaveChangesAsync(ct);
        return d.ToDto();
    }

    public async Task<IReadOnlyList<DesparasitacionResponse>> ListarDesparasitacionesPorAnimalAsync(int idAnimal, CancellationToken ct = default)
        => (await _despaRepo.ListByAnimalAsync(AnimalId.From(idAnimal), ct)).Select(d => d.ToDto()).ToList();
}

public sealed class ComplementoService : IComplementoService
{
    private readonly IComplementoRepository _repo;
    private readonly IUnitOfWork _uow;
    public ComplementoService(IComplementoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<ComplementoResponse> RegistrarAsync(RegistrarComplementoRequest req, CancellationToken ct = default)
    {
        var c = Complemento.Registrar(HistorialCurativoId.From(req.IdTratamiento), req.Fecha, req.Descripcion, req.Tipo, req.Costo);
        await _repo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<IReadOnlyList<ComplementoResponse>> ListarPorTratamientoAsync(int idTratamiento, CancellationToken ct = default)
        => (await _repo.ListPorTratamientoAsync(HistorialCurativoId.From(idTratamiento), ct)).Select(c => c.ToDto()).ToList();
}
