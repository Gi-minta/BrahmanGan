using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IContratoService
{
    Task<ContratoResponse> CrearAsync(CrearContratoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ContratoResponse>> ListarPorClienteAsync(int idCliente, CancellationToken ct = default);
}
