using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Comercial;

namespace BrahmanGan.Application.Ports.Output;

public interface IContratoRepository : IRepository<Contrato, ContratoId>
{
    Task<IReadOnlyList<Contrato>> ListByClienteAsync(ClienteId idCliente, CancellationToken ct = default);
}
