using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Nomina;

namespace BrahmanGan.Application.Ports.Output;

// ===== Fase 8: Nómina =====
public interface ITrabajadorRepository : IRepository<Trabajador, TrabajadorId>
{
    Task<Trabajador?> GetByCedulaAsync(string cedula, CancellationToken ct = default);
    Task<IReadOnlyList<Trabajador>> ListActivosAsync(CancellationToken ct = default);
}

public interface IPagoJornalRepository : IRepository<PagoJornal, PagoJornalId>
{
    Task<IReadOnlyList<PagoJornal>> ListPorTrabajadorAsync(TrabajadorId idTrabajador, CancellationToken ct = default);
    Task<IReadOnlyList<PagoJornal>> ListPorPeriodoAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default);
}
