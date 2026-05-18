using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sostenibilidad;

namespace BrahmanGan.Application.Ports.Output;

// ===== Fase 8: Sostenibilidad =====
public interface ICapturaCarbonoRepository : IRepository<CapturaCarbono, CapturaCarbonoId>
{
    Task<IReadOnlyList<CapturaCarbono>> ListPorFincaAsync(FincaId idFinca, CancellationToken ct = default);
}

public interface IConsumoAguaRepository : IRepository<ConsumoAgua, ConsumoAguaId>
{
    Task<IReadOnlyList<ConsumoAgua>> ListPorFincaAsync(FincaId idFinca, CancellationToken ct = default);
}

public interface IEventoMedioambientalRepository : IRepository<EventoMedioambiental, EventoMedioambientalId>
{
    Task<IReadOnlyList<EventoMedioambiental>> ListPorFincaAsync(FincaId idFinca, CancellationToken ct = default);
}
