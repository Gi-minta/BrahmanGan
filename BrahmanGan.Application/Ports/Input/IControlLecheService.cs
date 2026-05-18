using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// Fase 5
public interface IControlLecheService
{
    Task<ControlLecheResponse> RegistrarAsync(RegistrarControlLecheRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ControlLecheResponse>> ListarPorAnimalAsync(int idAnimal, DateOnly desde, DateOnly hasta, CancellationToken ct = default);

    Task<ParametroLactanciaResponse> IniciarLactanciaAsync(IniciarParametroLactanciaRequest req, CancellationToken ct = default);
    Task<ParametroLactanciaResponse> CerrarLactanciaAsync(int id, CerrarParametroLactanciaRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ParametroLactanciaResponse>> ListarLactanciasPorAnimalAsync(int idAnimal, CancellationToken ct = default);

    Task<CalidadLecheResponse> RegistrarCalidadAsync(RegistrarCalidadLecheRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<CalidadLecheResponse>> ListarCalidadPorFechaAsync(DateOnly desde, DateOnly hasta, CancellationToken ct = default);
}
