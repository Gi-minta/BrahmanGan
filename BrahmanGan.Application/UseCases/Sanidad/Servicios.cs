using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Application.UseCases.Sanidad;

public sealed class MedicamentoService : IMedicamentoService
{
    private readonly IMedicamentoRepository _repo;
    private readonly IUnitOfWork _uow;
    public MedicamentoService(IMedicamentoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

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
}

public sealed class VacunacionService : IVacunacionService
{
    private readonly IHistorialVacunacionRepository _repo;
    private readonly IUnitOfWork _uow;
    public VacunacionService(IHistorialVacunacionRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

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
}
