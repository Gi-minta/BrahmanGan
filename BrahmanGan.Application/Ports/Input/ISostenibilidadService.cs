using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// ===== Fase 8: Sostenibilidad =====
public interface ISostenibilidadService
{
    Task<CapturaCarbonoResponse> RegistrarCapturaAsync(RegistrarCapturaCarbonoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<CapturaCarbonoResponse>> ListarCapturasPorFincaAsync(int idFinca, CancellationToken ct = default);

    Task<ConsumoAguaResponse> RegistrarConsumoAguaAsync(RegistrarConsumoAguaRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ConsumoAguaResponse>> ListarConsumoAguaPorFincaAsync(int idFinca, CancellationToken ct = default);

    Task<EventoMedioambientalResponse> RegistrarEventoAsync(RegistrarEventoMedioambientalRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<EventoMedioambientalResponse>> ListarEventosPorFincaAsync(int idFinca, CancellationToken ct = default);
}
