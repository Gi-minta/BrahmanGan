using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Comercial;

namespace BrahmanGan.Application.Ports.Output;

// Fase 6
public interface IClienteRepository : IRepository<Cliente, ClienteId>
{
    Task<Cliente?> GetByDocumentoAsync(string documento, CancellationToken ct = default);
}
