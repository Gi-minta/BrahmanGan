using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Application.UseCases;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Almacen;

namespace BrahmanGan.Application.UseCases.Almacen;

public sealed class InsumoService : IInsumoService
{
    private readonly IInsumoRepository _repo;
    private readonly IKardexInsumoRepository _kardexRepo;
    private readonly IUnitOfWork _uow;
    public InsumoService(IInsumoRepository repo, IKardexInsumoRepository kardexRepo, IUnitOfWork uow)
    {
        _repo = repo; _kardexRepo = kardexRepo; _uow = uow;
    }

    public async Task<InsumoResponse> CrearAsync(CrearInsumoRequest req, CancellationToken ct = default)
    {
        var insumo = Insumo.Crear(req.Codigo, req.Nombre, req.Tipo, req.UnidadMedida,
            req.PrecioUnitario, req.StockMinimo, req.StockInicial);
        await _repo.AddAsync(insumo, ct);
        await _uow.SaveChangesAsync(ct);
        return insumo.ToDto();
    }

    public async Task<InsumoResponse?> ObtenerAsync(int id, CancellationToken ct = default)
    {
        var insumo = await _repo.GetByIdAsync(InsumoId.From(id), ct);
        return insumo?.ToDto();
    }

    public async Task<IReadOnlyList<InsumoResponse>> ListarAsync(CancellationToken ct = default)
        => (await _repo.ListActivosAsync(ct)).Select(i => i.ToDto()).ToList();

    public async Task<IReadOnlyList<InsumoResponse>> ListarBajoMinimoAsync(CancellationToken ct = default)
        => (await _repo.ListBajoMinimoAsync(ct)).Select(i => i.ToDto()).ToList();

    public async Task<KardexInsumoResponse> RegistrarMovimientoAsync(RegistrarMovimientoKardexRequest req, CancellationToken ct = default)
    {
        var insumo = await _repo.GetByIdAsync(InsumoId.From(req.IdInsumo), ct)
            ?? throw new DomainException("Insumo no encontrado");

        var tipo = Enum.Parse<TipoMovimientoKardex>(req.TipoMovimiento, ignoreCase: true);
        var (saldoAnt, saldoNuevo) = insumo.AplicarMovimiento(tipo, req.Cantidad);

        _repo.Update(insumo);

        var kardex = KardexInsumo.Registrar(insumo.Id, req.Fecha, tipo, req.Cantidad,
            saldoAnt, saldoNuevo, req.CostoUnitario, req.Concepto, req.Referencia);
        await _kardexRepo.AddAsync(kardex, ct);
        await _uow.SaveChangesAsync(ct);
        return kardex.ToDto();
    }

    public async Task<IReadOnlyList<KardexInsumoResponse>> ListarKardexAsync(int idInsumo, CancellationToken ct = default)
        => (await _kardexRepo.ListPorInsumoAsync(InsumoId.From(idInsumo), ct)).Select(k => k.ToDto()).ToList();
}
