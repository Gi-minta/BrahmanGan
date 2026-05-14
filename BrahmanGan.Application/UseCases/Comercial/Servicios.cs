using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.Modulos.Comercial;
using BrahmanGan.Domain.Modulos.Finca;
using BrahmanGan.Domain.ValueObjects;

namespace BrahmanGan.Application.UseCases.Comercial;

public sealed class ClienteService : IClienteService
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _uow;
    public ClienteService(IClienteRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<ClienteResponse> CrearAsync(CrearClienteRequest req, CancellationToken ct = default)
    {
        if (await _repo.GetByDocumentoAsync(req.Documento, ct) is not null)
            throw new BusinessRuleException($"Ya existe un cliente con documento '{req.Documento}'");
        var email = string.IsNullOrWhiteSpace(req.Email) ? null : Email.Create(req.Email);
        var c = Cliente.Crear(req.Documento, req.RazonSocial, req.TipoDocumento,
            req.Contacto, req.Telefono, email, req.Direccion,
            req.IdMunicipio is int m ? MunicipioId.From(m) : null, req.TipoCliente);
        await _repo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<ClienteResponse?> ObtenerAsync(int id, CancellationToken ct = default)
        => (await _repo.GetByIdAsync(ClienteId.From(id), ct))?.ToDto();

    public async Task<IReadOnlyList<ClienteResponse>> ListarAsync(CancellationToken ct = default)
        => (await _repo.ListAllAsync(ct)).Select(c => c.ToDto()).ToList();
}

public sealed class ContratoService : IContratoService
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;
    public ContratoService(IContratoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<ContratoResponse> CrearAsync(CrearContratoRequest req, CancellationToken ct = default)
    {
        var c = Contrato.Crear(ClienteId.From(req.IdCliente), req.Tipo, req.FechaInicio, req.FechaFin,
            req.PrecioAcordado, req.UnidadPrecio, req.VolumenEstimado, req.Condiciones);
        await _repo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task<IReadOnlyList<ContratoResponse>> ListarPorClienteAsync(int idCliente, CancellationToken ct = default)
        => (await _repo.ListByClienteAsync(ClienteId.From(idCliente), ct)).Select(c => c.ToDto()).ToList();
}

public sealed class CotizacionVentaService : ICotizacionVentaService
{
    private readonly ICotizacionVentaRepository _repo;
    private readonly IUnitOfWork _uow;
    public CotizacionVentaService(ICotizacionVentaRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<CotizacionResponse> CrearAsync(CrearCotizacionRequest req, CancellationToken ct = default)
    {
        var c = CotizacionVenta.Crear(ClienteId.From(req.IdCliente), req.Fecha, req.PrecioOfertado,
            req.FechaVigencia, req.UnidadPrecio, req.Observaciones);
        await _repo.AddAsync(c, ct);
        await _uow.SaveChangesAsync(ct);
        return c.ToDto();
    }

    public async Task AgregarDetalleAsync(int idCotizacion, AgregarDetalleCotizacionRequest req, CancellationToken ct = default)
    {
        var c = await _repo.GetByIdAsync(CotizacionVentaId.From(idCotizacion), ct)
            ?? throw new EntityNotFoundException(nameof(CotizacionVenta), idCotizacion);
        c.AgregarDetalle(AnimalId.From(req.IdAnimal), req.PesoEstimadoKg, req.PrecioUnitario);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task AprobarAsync(int idCotizacion, CancellationToken ct = default)
    {
        var c = await _repo.GetByIdAsync(CotizacionVentaId.From(idCotizacion), ct)
            ?? throw new EntityNotFoundException(nameof(CotizacionVenta), idCotizacion);
        c.Aprobar();
        await _uow.SaveChangesAsync(ct);
    }

    public async Task RechazarAsync(int idCotizacion, CancellationToken ct = default)
    {
        var c = await _repo.GetByIdAsync(CotizacionVentaId.From(idCotizacion), ct)
            ?? throw new EntityNotFoundException(nameof(CotizacionVenta), idCotizacion);
        c.Rechazar();
        await _uow.SaveChangesAsync(ct);
    }

    public async Task<CotizacionResponse?> ObtenerAsync(int idCotizacion, CancellationToken ct = default)
        => (await _repo.GetByIdAsync(CotizacionVentaId.From(idCotizacion), ct))?.ToDto();
}
