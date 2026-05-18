using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Application.UseCases;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Equipos;

namespace BrahmanGan.Application.UseCases.Equipos;

public sealed class MaquinariaService : IMaquinariaService
{
    private readonly IMaquinariaRepository _repo;
    private readonly IMantenimientoEquipoRepository _mantRepo;
    private readonly IUnitOfWork _uow;
    public MaquinariaService(IMaquinariaRepository repo, IMantenimientoEquipoRepository mantRepo, IUnitOfWork uow)
    {
        _repo = repo; _mantRepo = mantRepo; _uow = uow;
    }

    public async Task<MaquinariaResponse> CrearAsync(CrearMaquinariaRequest req, CancellationToken ct = default)
    {
        var m = Maquinaria.Crear(CentroCostoId.From(req.IdCentro), req.Codigo, req.Nombre,
            req.Marca, req.Modelo, req.Anio, req.NumeroSerie, req.FechaCompra, req.ValorCompra);
        await _repo.AddAsync(m, ct);
        await _uow.SaveChangesAsync(ct);
        return m.ToDto();
    }

    public async Task<MaquinariaResponse?> ObtenerAsync(int id, CancellationToken ct = default)
    {
        var m = await _repo.GetByIdAsync(MaquinariaId.From(id), ct);
        return m?.ToDto();
    }

    public async Task<IReadOnlyList<MaquinariaResponse>> ListarAsync(CancellationToken ct = default)
        => (await _repo.ListActivasAsync(ct)).Select(m => m.ToDto()).ToList();

    public async Task<MantenimientoEquipoResponse> RegistrarMantenimientoAsync(RegistrarMantenimientoRequest req, CancellationToken ct = default)
    {
        var maq = await _repo.GetByIdAsync(MaquinariaId.From(req.IdMaquinaria), ct)
            ?? throw new DomainException("Maquinaria no encontrada");

        var tipo = Enum.Parse<TipoMantenimiento>(req.TipoMantenimiento, ignoreCase: true);
        var mant = MantenimientoEquipo.Registrar(maq.Id, req.Fecha, tipo, req.Descripcion,
            req.CostoManoObra, req.CostoRepuestos, req.Tecnico, req.Proveedor,
            req.ProximoMantenimiento, req.HorasAlMomento);

        if (req.HorasAlMomento.HasValue)
        {
            var horasAdicionales = req.HorasAlMomento.Value - maq.HorasUso;
            if (horasAdicionales > 0) maq.RegistrarHoras(horasAdicionales);
        }

        _repo.Update(maq);
        await _mantRepo.AddAsync(mant, ct);
        await _uow.SaveChangesAsync(ct);
        return mant.ToDto();
    }

    public async Task<IReadOnlyList<MantenimientoEquipoResponse>> ListarMantenimientosAsync(int idMaquinaria, CancellationToken ct = default)
        => (await _mantRepo.ListPorMaquinariaAsync(MaquinariaId.From(idMaquinaria), ct)).Select(m => m.ToDto()).ToList();
}
