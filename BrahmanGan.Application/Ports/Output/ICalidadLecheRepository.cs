using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Application.Ports.Output;

public interface ICalidadLecheRepository : IRepository<CalidadLeche, CalidadLecheId>
{
    Task<IReadOnlyList<CalidadLeche>> ListPorPeriodoAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default);
}
